using System;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Items.Interactor
{
    public class InteractorBanzaiTimer : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
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
                Item.ExtraData = "0";
                oldValue = 0;
            }

            if (Request == 0 && oldValue == 0)
            {
                oldValue = 30;
            }
            else if (Request == 2)
            {
                if (Item.GetRoom().GetBanzai().isBanzaiActive && Item.pendingReset && oldValue > 0)
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

                if (!Item.GetRoom().GetBanzai().isBanzaiActive)
                {
                    Item.UpdateNeeded = !Item.UpdateNeeded;

                    if (Item.UpdateNeeded)
                    {
                        Item.GetRoom().GetBanzai().BanzaiStart();
                    }

                    Item.pendingReset = true;
                }
                else
                {
                    Item.UpdateNeeded = !Item.UpdateNeeded;

                    if (Item.UpdateNeeded)
                    {
                        Item.GetRoom().GetBanzai().BanzaiEnd(true);
                    }

                    Item.pendingReset = true;
                }
            }


            Item.ExtraData = Convert.ToString(oldValue);
            Item.UpdateState();
        }

        public void OnWiredTrigger(Item Item)
        {
            if (Item.GetRoom().GetBanzai().isBanzaiActive)
                Item.GetRoom().GetBanzai().BanzaiEnd(true);

            // Fixado Por Thiago Araujo

            bool TempoSpace = false;

            if (Item.ExtraData == "30" || Item.ExtraData == "60" || Item.ExtraData == "120" || Item.ExtraData == "180" || Item.ExtraData == "300" || Item.ExtraData == "600" && TempoSpace == false)
            {
                Item.UpdateNeeded = true;
                TempoSpace = true;
            }

            if (Item.ExtraData == "0" && TempoSpace == true)
            {
                Item.pendingReset = true;
                TempoSpace = false;
            }

            Item.UpdateState();

            if (!Item.GetRoom().GetBanzai().isBanzaiActive)
                Item.GetRoom().GetBanzai().BanzaiStart();
        }
    }
}
