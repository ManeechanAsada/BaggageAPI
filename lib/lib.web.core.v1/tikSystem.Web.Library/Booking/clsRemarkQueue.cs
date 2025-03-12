using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikSystem.Web.Library
{
    public class RemarkQueue
    {
        int _sort_sequence;
        public int sort_sequence
        {
            get { return _sort_sequence; }
            set { _sort_sequence = value; }
        }

        byte _is_event;
        public byte is_event
        {
            get { return _is_event; }
            set { _is_event = value; }
        }

        byte _within_48;
        public byte within_48
        {
            get { return _within_48; }
            set { _within_48 = value; }
        }

        string _remark_type_rcd;
        public string remark_type_rcd
        {
            get { return _remark_type_rcd; }
            set { _remark_type_rcd = value; }
        }

        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }

        int _queue_count;
        public int queue_count
        {
            get { return _queue_count; }
            set { _queue_count = value; }
        }
    }
}
