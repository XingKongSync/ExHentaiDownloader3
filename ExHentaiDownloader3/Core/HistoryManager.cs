using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Core
{
    public class HistoryManager : BindableBase
    {
        private static readonly Lazy<HistoryManager> _instance = new Lazy<HistoryManager>(() => new HistoryManager());

        public static HistoryManager Instance { get { return _instance.Value; } }

        public ObservableCollection<string> History { get => ConfigManager.Instance.Config.History; }

        public bool NeedSave { get; private set; } = false;

        public void AddItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item) || History.Contains(item))
                return;

            History.Insert(0, item);
            if (History.Count > 20)
            {
                History.RemoveAt(History.Count - 1);
            }
            NeedSave= true;
        }

        public void Clear()
        {
            History.Clear();
            NeedSave= true;
        }
    }
}
