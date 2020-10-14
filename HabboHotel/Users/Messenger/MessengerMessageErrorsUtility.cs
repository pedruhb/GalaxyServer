namespace Galaxy.HabboHotel.Users.Messenger
{
    public static class MessengerMessageErrorsUtility
    {
        public static int GetMessageErrorPacketNum(MessengerMessageErrors error)
        {
            switch (error)
            {
                default:
                case MessengerMessageErrors.FRIEND_MUTED:
                    return 3;

                case MessengerMessageErrors.YOUR_MUTED:
                    return 4;

                case MessengerMessageErrors.FRIEND_NOT_ONLINE:
                    return 5;

                case MessengerMessageErrors.YOUR_NOT_FRIENDS:
                    return 6;

                case MessengerMessageErrors.FRIEND_BUSY:
                    return 7;

                case MessengerMessageErrors.OFFLINE_FAILED:
                    return 10;
            }
        }
    }
}
