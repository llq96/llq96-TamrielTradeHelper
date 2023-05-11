using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using VladB.Utility;

namespace TamrielTradeApp
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }

        public readonly BrowserHelper BrowserHelper = new();
        public readonly ItemList ItemList = new();
        public readonly SearchList SearchList = new();
        public readonly ToolTip UrlToolTip = new();
        public readonly Updater Updater = new();

        public Form1()
        {
            Instance = this;

            BrowserHelper.Init();
            InitializeComponent();

            splitContainer1.Panel2.Controls.Add(BrowserHelper.BrowserView);
            BrowserHelper.BrowserView.LoadingStateChanged += BrowserViewOnLoadingStateChanged;
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;

            NextSearch();

            Updater.Init();
        }

        private void BrowserViewOnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            if (args.IsLoading == false)
            {
                StartHTMLParsing();
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != dataGridView1.Columns["ButtonHide"]!.Index)
            {
                return;
            }

            string guid = dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["GUID"]!.Index].Value.ToString();
            var item = ItemList.GetItemWithGUID(guid);
            item.IsHidden = !item.IsHidden;

            UpdateUI_ItemList();
        }

        public void NextSearch()
        {
            SearchList.NextSearchQuery();

            LinkGenerator.FullInfo info = SearchList.CurrentSearch.InfoForLink;
            label_loadingNowInfo.Text =
                "Поиск :" + "\n"
                          + $"{info.BaseInfo.NamePattern}" + "\n"
                          + $"Качество: {info.BaseInfo.Quality}" + "\n"
                          + (info.PriceInfo.IsActiveRestrictions
                              ? $"Цена от {info.PriceInfo.PriceMin} до {info.PriceInfo.PriceMax}"
                              : "Цена любая") + "\n"
                          + (info.AmountInfo.IsActiveRestrictions
                              ? $"Количество от {info.AmountInfo.AmountMin} до {info.AmountInfo.AmountMax}"
                              : "Количество любое") + "\n"
                          + (info.LevelInfo.IsActiveRestrictions
                              ? $"Уровень({(info.LevelInfo.IsChampionPointsType ? "ЧП" : "Обычный")}) от {info.LevelInfo.LevelMin} до {info.LevelInfo.LevelMax}"
                              : "Уровень любой");
            UrlToolTip.SetToolTip(linkLabelButton_copyURL, SearchList.CurrentSearch.GetURL());

            // browserHelper.browser.Navigation.LoadUrl(searchList.currentSearch.GetURL());
            Debug.WriteLine($"LoadUrl{SearchList.CurrentSearch.GetURL()}");
            BrowserHelper.BrowserView.LoadUrl(SearchList.CurrentSearch.GetURL());
        }

        private async void StartHTMLParsing()
        {
            Debug.WriteLine($"Loaded");
            await Task.Delay(4000);
            // Thread.Sleep(4000);

            Invoke(ParseHTMLNow);
            BrowserHelper.BrowserView.Stop();

            await Task.Delay(3000);
            // Thread.Sleep(3000);

            Invoke(NextSearch);
        }

        private void ParseHTMLNow()
        {
            BrowserHelper?.BrowserView?.GetSourceAsync().ContinueWith(taskHtml =>
            {
                Invoke(() =>
                {
                    string html = taskHtml.Result;
                    List<ItemInfo> list = new HtmlParser().ParseHTML(html);
                    ItemList.AddItems(list);
                });
            });
        }

        public void UpdateUI_ItemList(bool isUpdateList = true)
        {
            if (isUpdateList)
            {
                ItemList.SortList();
            }

            dataGridView1.Rows.Clear();

            ItemList.Items.Where(it => !it.IsTrash()).Act(item =>
            {
                dataGridView1.Rows.Add(
                    item.Image?.Clone(),
                    item.Name,
                    $"{item.Place}\n{item.Guild}",
                    $"{$"{item.OnePrice,12:N0}".Replace(',', ' ')}",
                    $"{item.Count}",
                    $"{$"{item.FullPrice,12:N0}".Replace(',', ' ')}",
                    $"{item.Time}",
                    item.IsHidden ? "Показывать" : "Скрывать",
                    $"{item.GUID}"
                );

                dataGridView1.Rows[^1].Cells[0].Style.BackColor = ItemInfo.QualityColors[(int)item.Quality];
                dataGridView1.Rows[^1].Cells[1].Style.BackColor = ItemInfo.QualityColors[(int)item.Quality];

                if (item.TimeMinutes <= 15)
                {
                    dataGridView1.Rows[^1].Cells[6].Style.BackColor = Color.FromArgb(255, 100, 210, 100);
                }
                else if (item.TimeMinutes <= 30)
                {
                    dataGridView1.Rows[^1].Cells[6].Style.BackColor = Color.FromArgb(255, 215, 225, 120);
                }
            });

            Updater.timer_updateItemList.TimerSetActive(Updater.timer_updateItemList.IsTimerActive, true);
        }

        private void LinkLabelBtn_UnHideAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ItemList.Items.Count(it => it.IsHidden) != 0)
            {
                ItemList.SetAllIsHidden(false);
                UpdateUI_ItemList();
            }
        }

        private void LinkLabelButton_copyURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(SearchList.CurrentSearch.GetURL());
        }

        private void BtnUpdateItemsList_Click(object sender, EventArgs e)
        {
            UpdateUI_ItemList();
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = !groupBox1.Visible;
            menuStrip1.Items[0].Text = groupBox1.Visible ? "Настройки(Скрыть)" : "Настройки(Показать)";
        }
    }
}