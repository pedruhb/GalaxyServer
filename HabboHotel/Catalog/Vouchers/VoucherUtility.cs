namespace Galaxy.HabboHotel.Catalog.Vouchers
{
    public static class VoucherUtility
    {
        public static VoucherType GetType(string Type)
        {
            switch (Type)
            {
                default:
                case "credit":
                    return VoucherType.CREDIT;
				case "ducket":
					return VoucherType.DUCKET;
				case "diamond":
					return VoucherType.DIAMOND;
				case "gotw":
					return VoucherType.GOTW;
				case "furni":
					return VoucherType.ITEM;
			}
        }

        public static string FromType(VoucherType Type)
        {
            switch (Type)
            {
                default:
				case VoucherType.CREDIT:
					return "credit";
				case VoucherType.DUCKET:
					return "ducket";
				case VoucherType.DIAMOND:
					return "diamond";
				case VoucherType.GOTW:
					return "gotw";
				case VoucherType.ITEM:
					return "furni";
			}
        }
    }
}
