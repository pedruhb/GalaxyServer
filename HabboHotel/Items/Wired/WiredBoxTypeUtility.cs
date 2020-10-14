using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Galaxy.HabboHotel.Items.Wired
{
    static class WiredBoxTypeUtility
    {
        public static WiredBoxType FromWiredId(int Id)
        {
            switch (Id)
            {
                default:
                    return WiredBoxType.None;
                case 1:
                    return WiredBoxType.TriggerUserSays;
                case 2:
                    return WiredBoxType.TriggerStateChanges;
                case 3:
                    return WiredBoxType.TriggerRepeat;
                case 4:
                    return WiredBoxType.TriggerRoomEnter;
                case 5:
                    return WiredBoxType.EffectShowMessage;
                case 6:
                    return WiredBoxType.EffectTeleportToFurni;
                case 7:
                    return WiredBoxType.EffectToggleFurniState;
                case 8:
                    return WiredBoxType.TriggerWalkOnFurni;
                case 9:
                    return WiredBoxType.TriggerWalkOffFurni;
                case 10:
                    return WiredBoxType.EffectKickUser;
                case 11:
                    return WiredBoxType.ConditionFurniHasUsers;
                case 12:
                    return WiredBoxType.ConditionFurniHasFurni;
                case 13:
                    return WiredBoxType.ConditionTriggererOnFurni;
                case 14:
                    return WiredBoxType.EffectMatchPosition;
                case 21:
                    return WiredBoxType.ConditionIsGroupMember;
                case 22:
                    return WiredBoxType.ConditionIsNotGroupMember;
                case 23:
                    return WiredBoxType.ConditionTriggererNotOnFurni;
                case 24:
                    return WiredBoxType.ConditionFurniHasNoUsers;
                case 25:
                    return WiredBoxType.ConditionIsWearingBadge;
                case 26:
                    return WiredBoxType.ConditionIsWearingFX;
                case 27:
                    return WiredBoxType.ConditionIsNotWearingBadge;
                case 28:
                    return WiredBoxType.ConditionIsNotWearingFX;
                case 29:
                    return WiredBoxType.ConditionMatchStateAndPosition;
                case 30:
                    return WiredBoxType.ConditionUserCountInRoom;
                case 31:
                    return WiredBoxType.ConditionUserCountDoesntInRoom;
                case 32:
                    return WiredBoxType.EffectMoveAndRotate;
                case 33:
                    return WiredBoxType.ConditionDontMatchStateAndPosition;
                case 34:
                    return WiredBoxType.ConditionFurniTypeMatches;
                case 35:
                    return WiredBoxType.ConditionFurniTypeDoesntMatch;
                case 36:
                    return WiredBoxType.ConditionFurniHasNoFurni;
                case 37:
                    return WiredBoxType.EffectMoveFurniToNearestUser;
                case 38:
                    return WiredBoxType.EffectMoveFurniFromNearestUser;
                case 39:
                    return WiredBoxType.EffectMuteTriggerer;
                case 40:
                    return WiredBoxType.EffectGiveReward;
                case 41:
                    return WiredBoxType.AddonRandomEffect;
                case 42:
                    return WiredBoxType.TriggerGameStarts;
                case 43:
                    return WiredBoxType.TriggerGameEnds;
                case 44:
                    return WiredBoxType.TriggerUserFurniCollision;
                case 45:
                    return WiredBoxType.EffectMoveFurniToNearestUser;
                case 46:
                    return WiredBoxType.EffectExecuteWiredStacks;
                case 47:
                    return WiredBoxType.EffectTeleportBotToFurniBox;
                case 48:
                    return WiredBoxType.EffectBotChangesClothesBox;
                case 49:
                    return WiredBoxType.EffectBotMovesToFurniBox;
                case 50:
                    return WiredBoxType.EffectBotCommunicatesToAllBox;
                case 51:
                    return WiredBoxType.EffectBotCommunicatesToUserBox;
                case 52:
                    return WiredBoxType.EffectBotFollowsUserBox;
                case 53:
                    return WiredBoxType.EffectBotGivesHanditemBox;
                case 54:
                    return WiredBoxType.ConditionActorHasHandItemBox;
                case 55:
                    return WiredBoxType.ConditionActorIsInTeamBox;
                case 56:
                    return WiredBoxType.EffectAddActorToTeam;
                case 57:
                    return WiredBoxType.EffectRemoveActorFromTeam;
                case 58:
                    return WiredBoxType.TriggerUserSaysCommand;
                case 59:
                    return WiredBoxType.EffectSetRollerSpeed;
                case 60:
                    return WiredBoxType.EffectRegenerateMaps;
                case 61:
                    return WiredBoxType.EffectGiveUserBadge;
                case 62:
                    return WiredBoxType.EffectAddScore;
                case 63:
                    return WiredBoxType.TriggerLongRepeat;
                case 64:
                    return WiredBoxType.EffectHandUserItemBox;
                case 65:
                    return WiredBoxType.EffectEnableUserBox;
                case 66:
                    return WiredBoxType.EffectDanceUserBox;
                case 67:
                    return WiredBoxType.EffectFreezeUserBox;
                case 68:
                    return WiredBoxType.EffectFastWalkUserBox;
                case 69:
                    return WiredBoxType.SendCustomMessageBox;
                case 70:
                    return WiredBoxType.ConditionActorIsNotInTeamBox;
                case 72:
                    return WiredBoxType.ConditionActorHasNotHandItemBox;
                case 73:
                    return WiredBoxType.EffectAddRewardPoints;
                case 74:
                    return WiredBoxType.ConditionDateRangeActive;
                case 75:
                    return WiredBoxType.EffectApplyClothes;
                case 76:
                    return WiredBoxType.ConditionWearingClothes;
                case 77:
                    return WiredBoxType.ConditionNotWearingClothes;
                case 78:
                    return WiredBoxType.TriggerAtGivenTime;
                case 79:
                    return WiredBoxType.EffectMoveUser;
                case 80:
                    return WiredBoxType.EffectTimerReset;
                // case 81:
                //   return WiredBoxType.EffectMoveToDir;
                case 82:
                    return WiredBoxType.EffectToggleNegativeFurniState;
                case 83:
                    return WiredBoxType.EffectProgressUserAchievement;
                case 84:
                    return WiredBoxType.TotalUsersCoincidence;
                case 85:
                    return WiredBoxType.ConditionLessThanTimer;
                case 86:
                    return WiredBoxType.ConditionMoreThanTimer;
                case 87:
                    return WiredBoxType.TriggerBotReachUser;
                case 88:
                    return WiredBoxType.TriggerBotReachFurni;
                case 89:
                    return WiredBoxType.EffectCloseDice;
                case 90:
                    return WiredBoxType.EffectGiveFurni;
                case 91:
                    return WiredBoxType.ShowAlertPHBox;
                case 92:
                    return WiredBoxType.EffectSendToRoomUserBox;
                case 93:
                    return WiredBoxType.ConditionSpaceHasDiamonds; /// Diamantes
                case 94:
                    return WiredBoxType.ConditionSpaceHasCredits; /// Creditos
                case 95:
                    return WiredBoxType.ConditionSpaceHasMeteoritos;  // Meteoritos = Gotw points
                case 96:
                    return WiredBoxType.ConditionSpaceHasDuckets;  // duckets
                case 97:
                    return WiredBoxType.ConditionSpaceNoHasDiamonds; /// Diamantes negativo
                case 98:
                    return WiredBoxType.ConditionSpaceNoHasCredits; /// Creditos negativo
                case 99:
                    return WiredBoxType.ConditionSpaceNoHasMeteoritos;  // Meteoritos = Gotw points negativo
                case 100:
                    return WiredBoxType.ConditionSpaceNoHasDuckets;  // duckets negativo
                case 101:
                    return WiredBoxType.ConditionSpaceHasRank;  // rank positivo
                case 102:
                    return WiredBoxType.ConditionSpaceNoHasRank;  // rank negativo
                case 103:
                    return WiredBoxType.EffectGiveUserDiamondsBox; /// dar diamantes
                case 104:
                    return WiredBoxType.EffectGiveUserCreditsBox; /// dar creditos
                case 105:
                    return WiredBoxType.EffectGiveUserDucketsBox; /// dar duckets
                case 106:
                    return WiredBoxType.EffectMoveToDir;
                case 107:
                    return WiredBoxType.ConditionSpaceUserIdle; // Usuario ausente positivo
                case 108:
                    return WiredBoxType.EffectMoveFurniFromUserBox;
                case 109:
                    return WiredBoxType.EffectEnableTeleportUserBox; // Efeito Wired Custom: Ativa/Desativa teletransporte
                case 110:
                    return WiredBoxType.EffectGiveFurniUmaVezBox; // Wired Staff: Dar mobi ao usuário (Uma Vez)
                case 111:
                    return WiredBoxType.SendGrapicAlertPHB; // Wired Staff: Alerta de imagem
                case 112:
                    return WiredBoxType.EffectMoveUserToFurniBox; // Efeito Wired: Mover usuário até mobi selecionado
                case 113:
                    return WiredBoxType.TriggerUserAFKBox; // Ativador Wired: Usuário ausente
                case 114:
                    return WiredBoxType.EffectMoveUserBox; // Efeito Wired: Mover usuário
                case 115:
                    return WiredBoxType.TriggerSpaceFazGol; // Ativador Wired: Habbo faz gol
                case 116:
                    return WiredBoxType.EffectBotCopiaVisual; // Efeito Wired: Bot copia visual
                case 117:
                    return WiredBoxType.TriggerUserSaiDoQuarto; // Ativador Wired: Habbo sai do quarto
                case 118:
                    return WiredBoxType.ConditionDonoEstaNoQuarto; // Condição positiva: dono está no quarto
                case 119:
                    return WiredBoxType.ConditionDonoNaoEstaNoQuarto; // Condição negativa: dono não está no quarto
                case 120:
                    return WiredBoxType.EffectFixRoomBox;
				case 121:
					return WiredBoxType.SendVideoToUserBox;
				case 122:
					return WiredBoxType.TeleportFurniBox;
                case 123:
                    return WiredBoxType.EffectBotMudaMissao; // Efeito Wired: Bot muda missão

            }
        }

        public static int GetWiredId(WiredBoxType Type)
        {
            switch (Type)
            {
                case WiredBoxType.TriggerUserSays:
                case WiredBoxType.TriggerUserSaysCommand:
                case WiredBoxType.ConditionMatchStateAndPosition:


                    return 0;
                case WiredBoxType.TriggerWalkOnFurni:
                case WiredBoxType.TriggerWalkOffFurni:
                case WiredBoxType.ConditionFurniHasUsers:
                case WiredBoxType.TotalUsersCoincidence:
                case WiredBoxType.ConditionTriggererOnFurni:
                case WiredBoxType.EffectEnableTeleportUserBox:
                case WiredBoxType.EffectFixRoomBox:
                case WiredBoxType.TriggerSpaceFazGol:


                    return 1;
                case WiredBoxType.EffectMatchPosition:
                case WiredBoxType.TriggerAtGivenTime:
                case WiredBoxType.ConditionLessThanTimer:
                case WiredBoxType.ConditionMoreThanTimer:
                    return 3;
                case WiredBoxType.EffectMoveAndRotate:
                case WiredBoxType.TriggerStateChanges:
                case WiredBoxType.EffectMoveUser:
                case WiredBoxType.EffectMoveUserBox:
                    return 4;
                case WiredBoxType.ConditionUserCountInRoom:
                case WiredBoxType.EffectCloseDice:

                    return 5;
                case WiredBoxType.ConditionActorIsInTeamBox:
                case WiredBoxType.ConditionActorIsNotInTeamBox:
                case WiredBoxType.TriggerRepeat:
                case WiredBoxType.TriggerLongRepeat:
                case WiredBoxType.EffectAddScore:
                case WiredBoxType.TriggerUserAFKBox:
                    return 6;
                case WiredBoxType.TriggerRoomEnter:
                case WiredBoxType.TriggerUserSaiDoQuarto:
                case WiredBoxType.EffectShowMessage:
                case WiredBoxType.SendCustomMessageBox:
                case WiredBoxType.EffectProgressUserAchievement:
                case WiredBoxType.ConditionFurniHasFurni:

                    return 7;
                case WiredBoxType.TriggerGameStarts:
                case WiredBoxType.TriggerGameEnds:
                case WiredBoxType.EffectTeleportToFurni:
                case WiredBoxType.EffectToggleFurniState:
                case WiredBoxType.ConditionFurniTypeMatches:
                case WiredBoxType.EffectMoveUserToFurniBox:
                case WiredBoxType.EffectTimerReset:
				case WiredBoxType.TeleportFurniBox:


					return 8;
                case WiredBoxType.EffectGiveUserBadge:
                case WiredBoxType.EffectEnableUserBox:
                case WiredBoxType.EffectDanceUserBox:
                case WiredBoxType.EffectGiveUserCreditsBox:
                case WiredBoxType.EffectGiveUserDucketsBox:
                case WiredBoxType.EffectGiveUserDiamondsBox:
                case WiredBoxType.EffectHandUserItemBox:
                case WiredBoxType.EffectGiveFurni:
                case WiredBoxType.EffectGiveFurniUmaVezBox:
                case WiredBoxType.ShowAlertPHBox:
                case WiredBoxType.EffectRegenerateMaps:
                case WiredBoxType.EffectKickUser:
                case WiredBoxType.EffectSetRollerSpeed:
                case WiredBoxType.EffectSendToRoomUserBox:
                case WiredBoxType.SendGrapicAlertPHB:
				case WiredBoxType.SendVideoToUserBox:

					return 7;
                case WiredBoxType.EffectAddActorToTeam:

                    return 9;
                case WiredBoxType.EffectRemoveActorFromTeam:
                case WiredBoxType.ConditionIsGroupMember:
                case WiredBoxType.ConditionDonoEstaNoQuarto:
                case WiredBoxType.ConditionDonoNaoEstaNoQuarto:
                    return 10;
                case WiredBoxType.TriggerUserFurniCollision:
                case WiredBoxType.ConditionIsWearingBadge:
                case WiredBoxType.EffectMoveFurniToNearestUser:
                case WiredBoxType.EffectMoveFurniFromUserBox:
                case WiredBoxType.ConditionSpaceHasDiamonds:
                case WiredBoxType.ConditionSpaceHasCredits:
                case WiredBoxType.ConditionSpaceHasMeteoritos:
                case WiredBoxType.ConditionSpaceHasDuckets:
                case WiredBoxType.ConditionSpaceNoHasDiamonds:
                case WiredBoxType.ConditionSpaceNoHasCredits:
                case WiredBoxType.ConditionSpaceNoHasMeteoritos:
                case WiredBoxType.ConditionSpaceNoHasDuckets:
                case WiredBoxType.ConditionSpaceHasRank:
                case WiredBoxType.ConditionSpaceNoHasRank:
                case WiredBoxType.ConditionSpaceUserIdle:

                    return 11;
                case WiredBoxType.ConditionIsWearingFX:
                    return 12;

                case WiredBoxType.EffectMoveToDir:
                    return 13;

                case WiredBoxType.ConditionFurniHasNoUsers:
                case WiredBoxType.TriggerBotReachUser:
                case WiredBoxType.TriggerBotReachFurni:
               

                    return 14;
                case WiredBoxType.ConditionTriggererNotOnFurni:
                    return 15;
                case WiredBoxType.ConditionUserCountDoesntInRoom:


                    return 16;
                case WiredBoxType.EffectGiveReward:

                    return 17;
                case WiredBoxType.EffectExecuteWiredStacks:

                case WiredBoxType.ConditionFurniHasNoFurni:
                    return 18;
                case WiredBoxType.ConditionFurniTypeDoesntMatch:
                case WiredBoxType.EffectMoveFurniFromNearestUser:

                    return 19;
                case WiredBoxType.EffectMuteTriggerer:
                    return 20;
                case WiredBoxType.ConditionIsNotGroupMember:
                case WiredBoxType.EffectTeleportBotToFurniBox:
                    return 21;
                case WiredBoxType.ConditionIsNotWearingBadge:
                case WiredBoxType.ConditionActorHasHandItemBox:
                case WiredBoxType.ConditionActorHasNotHandItemBox:
                case WiredBoxType.ConditionWearingClothes:
                case WiredBoxType.ConditionNotWearingClothes:
                case WiredBoxType.EffectBotMovesToFurniBox:
                    return 22;
                case WiredBoxType.ConditionIsNotWearingFX:
                case WiredBoxType.EffectBotCommunicatesToAllBox:
                    return 23;
           //     case WiredBoxType.EffectBotGivesHanditemBox:
                case WiredBoxType.ConditionDateRangeActive:
                    return 24;
                case WiredBoxType.EffectBotFollowsUserBox:
                case WiredBoxType.EffectBotCopiaVisual:

                    return 25;
                case WiredBoxType.EffectBotChangesClothesBox:
                case WiredBoxType.EffectApplyClothes:

                    return 26;
                case WiredBoxType.EffectBotCommunicatesToUserBox:
                case WiredBoxType.EffectBotGivesHanditemBox:
                case WiredBoxType.EffectBotMudaMissao:

                    return 27;
            }
            return 0;
        }

        public static List<int> ContainsBlockedTrigger(IWiredItem Box, ICollection<IWiredItem> Triggers)
        {
            List<int> BlockedItems = new List<int>();

            if (Box.Type != WiredBoxType.EffectShowMessage && Box.Type != WiredBoxType.EffectMuteTriggerer && Box.Type != WiredBoxType.EffectTeleportToFurni && Box.Type != WiredBoxType.EffectKickUser && Box.Type != WiredBoxType.ConditionTriggererOnFurni)
                return BlockedItems;

            foreach (IWiredItem Item in Triggers)
            {
                if (Item.Type == WiredBoxType.TriggerRepeat || Item.Type == WiredBoxType.TriggerLongRepeat)
                {
                    if (!BlockedItems.Contains(Item.Item.GetBaseItem().SpriteId))
                        BlockedItems.Add(Item.Item.GetBaseItem().SpriteId);
                    else continue;
                }
                else continue;
            }

            return BlockedItems;
        }
        public static List<int> ContainsBlockedEffect(IWiredItem Box, ICollection<IWiredItem> Effects)
        {
            List<int> BlockedItems = new List<int>();

            if (Box.Type != WiredBoxType.TriggerRepeat || Box.Type != WiredBoxType.TriggerLongRepeat)
                return BlockedItems;

            bool HasMoveRotate = Effects.Where(x => x.Type == WiredBoxType.EffectMoveAndRotate).ToList().Count > 0;
            bool HasMoveNear = Effects.Where(x => x.Type == WiredBoxType.EffectMoveFurniToNearestUser).ToList().Count > 0;
            bool HasMoveToDir = Effects.Where(x => x.Type == WiredBoxType.EffectMoveToDir).ToList().Count > 0;

            foreach (IWiredItem Item in Effects)
            {
                if (Item.Type == WiredBoxType.EffectKickUser || Item.Type == WiredBoxType.EffectMuteTriggerer || Item.Type == WiredBoxType.EffectShowMessage || Item.Type == WiredBoxType.SendCustomMessageBox || Item.Type == WiredBoxType.EffectProgressUserAchievement || Item.Type == WiredBoxType.EffectTeleportToFurni || Item.Type == WiredBoxType.EffectBotFollowsUserBox || Item.Type == WiredBoxType.EffectBotCopiaVisual)
                {
                    if (!BlockedItems.Contains(Item.Item.GetBaseItem().SpriteId))
                        BlockedItems.Add(Item.Item.GetBaseItem().SpriteId);
                    else continue;
                }
                else if ((Item.Type == WiredBoxType.EffectMoveFurniToNearestUser && (HasMoveRotate || HasMoveNear || HasMoveToDir)) || (Item.Type == WiredBoxType.EffectMoveAndRotate && (HasMoveNear || HasMoveToDir)) || (Item.Type == WiredBoxType.EffectMoveToDir && (HasMoveNear || HasMoveRotate)))
                {
                    if (!BlockedItems.Contains(Item.Item.GetBaseItem().SpriteId))
                        BlockedItems.Add(Item.Item.GetBaseItem().SpriteId);
                    else continue;
                }
            }

            return BlockedItems;
        }
    }
}

