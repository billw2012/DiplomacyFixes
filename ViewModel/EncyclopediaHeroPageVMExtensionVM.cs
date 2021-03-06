﻿using DiplomacyFixes.GauntletInterfaces;
using DiplomacyFixes.GrantFief;
using DiplomacyFixes.Messengers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace DiplomacyFixes.ViewModel
{
    class EncyclopediaHeroPageVMExtensionVM : EncyclopediaHeroPageVM
    {
        private bool _isMessengerAvailable;
        private bool _canGrantFief;
        private readonly Hero _hero;

        private GrantFiefInterface _grantFiefInterface;

        public EncyclopediaHeroPageVMExtensionVM(EncyclopediaPageArgs args) : base(args)
        {
            this._grantFiefInterface = new GrantFiefInterface();
            _hero = (base.Obj as Hero);
            this.SendMessengerInfluenceCost = (int)DiplomacyCostCalculator.DetermineInfluenceCostForSendingMessenger();
            this.SendMessengerActionName = new TextObject("{=cXfcwzPp}Send Messenger").ToString();
            this.GrantFiefActionName = new TextObject("{=LpoyhORp}Grant Fief").ToString();
            this.CanGrantFief = GrantFiefAction.CanGrantFief(this._hero.Clan, out _);
            base.RefreshValues();
        }

        protected void SendMessenger()
        {
            Events.Instance.OnMessengerSent(_hero);
            UpdateIsMessengerAvailable();
        }

        private void ExecuteLink(string link)
        {
            Campaign.Current.EncyclopediaManager.GoToLink(link);
        }

        private void GrantFief()
        {
            _grantFiefInterface.ShowFiefInterface(ScreenManager.TopScreen, this._hero);
        }

        [DataSourceProperty]
        public bool CanGrantFief
        {
            get
            {
                return this._canGrantFief;
            }
            set
            {
                if (value != this._canGrantFief)
                {
                    this._canGrantFief = value;
                    base.OnPropertyChanged("CanGrantFief");
                }
            }
        }

        [DataSourceProperty]
        public int SendMessengerInfluenceCost { get; } = Settings.Instance.SendMessengerInfluenceCost;

        [DataSourceProperty]
        public bool IsMessengerAvailable
        {
            get
            {
                UpdateIsMessengerAvailable();
                return this._isMessengerAvailable;
            }
            set
            {
                if (value != this._isMessengerAvailable)
                {
                    this._isMessengerAvailable = value;
                    base.OnPropertyChanged("IsMessengerAvailable");
                }
            }
        }

        private void UpdateIsMessengerAvailable()
        {
            IsMessengerAvailable = MessengerManager.CanSendMessengerWithInfluenceCost(this._hero, SendMessengerInfluenceCost);
        }

        [DataSourceProperty]
        public string SendMessengerActionName { get; private set; }

        [DataSourceProperty]
        public string GrantFiefActionName { get; private set; }
    }
}
