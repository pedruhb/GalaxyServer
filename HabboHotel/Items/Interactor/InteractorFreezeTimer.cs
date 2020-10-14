using System;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Items.Interactor
{
    class InteractorFreezeTimer : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
            Item.UpdateState();
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (!HasRights)
            {
                return;
            }

            int oldValue = 0;

            if (!int.TryParse(Item.ExtraData, out oldValue))
            {
                Item.ExtraData = "30";
                oldValue = 30;
            }

            if (Request == 0 && oldValue == 0)
            {
                oldValue = 30;
            }
            else if (Request == 2)
            {
                if (Item.GetRoom().GetFreeze().GameIsStarted && Item.pendingReset && oldValue > 0)
                {
                    oldValue = 0;
                    Item.pendingReset = false;
                }
                else
                {
                    if (oldValue < 30)
                        oldValue = 30;
                    else if (oldValue == 30)
                        oldValue = 60;
                    else if (oldValue == 60)
                        oldValue = 120;
                    else if (oldValue == 120)
                        oldValue = 180;
                    else if (oldValue == 180)
                        oldValue = 300;
                    else if (oldValue == 300)
                        oldValue = 600;
                    else
                        oldValue = 0;
                    Item.UpdateNeeded = false;
                }
            }
            else if (Request == 1 || Request == 0)
            {
                if (Request == 1 && oldValue == 0)
                {
                    Item.ExtraData = "30";
                    oldValue = 30;
                }

                if (!Item.GetRoom().GetFreeze().GameIsStarted)
                {
                    Item.UpdateNeeded = !Item.UpdateNeeded;

                    if (Item.UpdateNeeded)
                    {
                        Item.GetRoom().GetFreeze().StartGame();
                    }

                    Item.pendingReset = true;
                }
                else
                {
                    Item.UpdateNeeded = !Item.UpdateNeeded;

                    if (Item.UpdateNeeded)
                    {
                        Item.GetRoom().GetFreeze().StopGame(true);
                    }

                    Item.pendingReset = true;
                }
            }


            Item.ExtraData = Convert.ToString(oldValue);
            Item.UpdateState();
        }

        public void OnWiredTrigger(Item Item)
        {
            if (Item.GetRoom().GetFreeze().GameIsStarted)
                Item.GetRoom().GetFreeze().StopGame(true);

            // Fixado Por Thiago Araujo

            bool TempoThiago = false;

            if (Item.ExtraData == "30" || Item.ExtraData == "60" || Item.ExtraData == "120" || Item.ExtraData == "180" || Item.ExtraData == "300" || Item.ExtraData == "600" && TempoThiago == false)
            {
                Item.UpdateNeeded = true;
                TempoThiago = true;
            }

            if (Item.ExtraData == "0" && TempoThiago == true)
            {
                Item.pendingReset = true;
                TempoThiago = false;
            }

            Item.UpdateState();

            Item.GetRoom().GetFreeze().StartGame();
        }
    }
}
