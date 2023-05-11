using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TamrielTradeApp
{
    public class Updater
    {
        private Form1 form => Form1.Instance;

        public readonly Timer Timer_UpdateBtnUnHide = new(1f);
        public readonly Timer Timer_UpdateProgressBar = new(0.05f);
        public readonly Timer Timer_UpdateItemList = new(5f);

        private List<Timer> Timers { get; set; }

        public void Init()
        {
            Timers = new List<Timer>()
            {
                Timer_UpdateBtnUnHide,
                Timer_UpdateProgressBar,
                Timer_UpdateItemList
            };

            Timer_UpdateBtnUnHide.OnEndTime += Update_BtnUnHide; //TODO Вынести
            Timer_UpdateItemList.OnEndTime += UpdateItemsUI;
            Timer_UpdateProgressBar.OnEndTime += UpdateProgressBar;

            Timers.ForEach(t => { t.TimerSetActive(true, true); });

            new TaskFactory().StartNew(async () => await UpdateTask());
        }

        private async Task UpdateTask(int deltaMilliseconds = 50)
        {
            while (true)
            {
                Stopwatch sw = Stopwatch.StartNew();
                await Task.Delay(deltaMilliseconds);
                sw.Stop();
                form.Invoke(() => UpdateTimers(sw.ElapsedMilliseconds / 1000f));
            }
        }

        private void UpdateTimers(float deltaTime)
        {
            Timers.ForEach(t => t.UpdateFunc(deltaTime));
        }

        private void Update_BtnUnHide()
        {
            form.linkLabelBtn_UnHideAll.Text =
                $"очистить список скрытого({form.ItemList.Items.Count(it => it.IsHidden)})";
        }

        private void UpdateItemsUI()
        {
            form.UpdateUI_ItemList();
        }

        private void UpdateProgressBar()
        {
            int value = (int)(100 * (1f - (Timer_UpdateItemList.CurrentTime / Timer_UpdateItemList.MaxTimeValue)));
            form.pictureBox2.Size = new System.Drawing.Size(value, 15);
        }
    }
}