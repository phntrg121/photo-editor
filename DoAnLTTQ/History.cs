using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public enum HistoryEvent
    {
        Draw, NewL, DeleteL, DuplicateL, MergeL, Lup, Ldown, Opacity, DrawFilter, Erase, Clear, Fill, Transform, Blend
    }
    public partial class History : UserControl
    {
        Stack<KeyValuePair<HistoryEvent, LayerRow>> events;
        public History()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            events = new Stack<KeyValuePair<HistoryEvent, LayerRow>>();
        }

        public void Add(HistoryEvent e, LayerRow l)
        {
            events.Push(new KeyValuePair<HistoryEvent, LayerRow>(e, l));
            string eventName = l.Layer.Name + ": ";
            switch(e)
            {
                case HistoryEvent.Draw:
                    eventName += "Draw";
                    break;
                case HistoryEvent.Erase:
                    eventName += "Erase";
                    break;
                case HistoryEvent.DrawFilter:
                    eventName += "Filter";
                    break;
                case HistoryEvent.NewL:
                    eventName += "New";
                    break;
                case HistoryEvent.DuplicateL:
                    eventName += "Duplicate";
                    break;
                case HistoryEvent.DeleteL:
                    eventName += "Delete";
                    break;
                case HistoryEvent.MergeL:
                    eventName += "Merge";
                    break;
                case HistoryEvent.Lup:
                    eventName += "Move up";
                    break;
                case HistoryEvent.Ldown:
                    eventName += "Move down";
                    break;
                case HistoryEvent.Opacity:
                    eventName += "Opacity change";
                    break;
                case HistoryEvent.Clear:
                    eventName += "Clear";
                    break;
                case HistoryEvent.Fill:
                    eventName += "Fill";
                    break;
                case HistoryEvent.Transform:
                    eventName += "Transform";
                    break;
                case HistoryEvent.Blend:
                    eventName += "Blendmode change";
                    break;
                default:
                    break;
            }
            listBox1.Items.Insert(0, eventName);
        }

        public bool Remove()
        {
            if (events.Count == 0)
                return false;

            Form1 form = (Form1)Parent.Parent.Parent.Parent;
            LayerContainer container = (LayerContainer)events.Peek().Value.Parent.Parent;

            switch (events.Peek().Key)
            {
                case HistoryEvent.Draw:
                case HistoryEvent.DrawFilter:
                case HistoryEvent.Erase:
                case HistoryEvent.Clear:
                case HistoryEvent.Fill:
                case HistoryEvent.Transform:
                    events.Peek().Value.Layer.UnStacking();
                    break;
                case HistoryEvent.NewL:
                case HistoryEvent.DuplicateL:
                    container.DeleteRowAt(events.Peek().Value);
                    form.LayerButtonCheck();
                    break;
                case HistoryEvent.DeleteL:
                    container.RestoreRow();
                    form.LayerButtonCheck();
                    break;
                case HistoryEvent.MergeL:
                    events.Peek().Value.Layer.UnStacking();
                    container.RestoreRow();
                    form.LayerButtonCheck();
                    break;
                case HistoryEvent.Lup:
                    container.MoveBack(events.Peek().Value, false);
                    break;
                case HistoryEvent.Ldown:
                    container.MoveBack(events.Peek().Value, true);
                    break;
                case HistoryEvent.Opacity:
                    events.Peek().Value.Layer.RestoreOpacity();
                    form.opacityVal = events.Peek().Value.Layer.Opacity;
                    events.Peek().Value.Opacity = events.Peek().Value.Layer.Opacity;
                    form.OpacityBarUpdate();
                    form.DSUpdate();
                    break;
                case HistoryEvent.Blend:
                    events.Peek().Value.RestoreBlend();
                    form.BlendModeBoxUpdate(events.Peek().Value.Blend);
                    form.DSUpdate();
                    break;
                default:
                    break;
            }
            events.Pop();
            listBox1.Items.RemoveAt(0);
            return true;
        }
    }
}
