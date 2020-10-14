using Galaxy.Core.FigureData.Types;
using Galaxy.Core.FigureDataManager;
using Galaxy.HabboHotel.Users.Clothing.Parts;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Galaxy.Core.FigureData
{
    public class FigureDataManager
    {
        private static readonly ILog log = LogManager.GetLogger("Galaxy.Core.FigureData");
        private bool _legacyEnabled;
        private readonly LegacyFigureMutant _legacy;
        private readonly List<string> _requirements;
        private readonly Dictionary<int, Palette> _palettes; //pallet id, Pallet
        private readonly Dictionary<string, FigureSet> _setTypes; //type (hr, ch, etc), Set
        public FigureDataManager(bool legacyEnabled)
        {
            this._legacyEnabled = legacyEnabled;
            this._legacy = new LegacyFigureMutant();
            this._palettes = new Dictionary<int, Palette>();
            this._setTypes = new Dictionary<string, FigureSet>();
            this._requirements = new List<string>();
            this._requirements.Add("hd");
            this._requirements.Add("ch");
            this._requirements.Add("lg");
        }
        public void Init()
        {
            if (this._legacyEnabled == true)
            {
                this._legacy.Init();
                log.Info("» Figure Manager -> PRONTO!.");
                return;
            }
            if (this._palettes.Count > 0)
                this._palettes.Clear();
            if (this._setTypes.Count > 0)
                this._setTypes.Clear();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"figuredata.xml");
            XmlNodeList Colors = xDoc.GetElementsByTagName("colors");
            foreach (XmlNode Node in Colors)
            {
                foreach (XmlNode Child in Node.ChildNodes)
                {
                    this._palettes.Add(Convert.ToInt32(Child.Attributes["id"].Value), new Palette(Convert.ToInt32(Child.Attributes["id"].Value)));
                    foreach (XmlNode Sub in Child.ChildNodes)
                    {
                        this._palettes[Convert.ToInt32(Child.Attributes["id"].Value)].Colors.Add(Convert.ToInt32(Sub.Attributes["id"].Value), new Color(Convert.ToInt32(Sub.Attributes["id"].Value), Convert.ToInt32(Sub.Attributes["index"].Value), Convert.ToInt32(Sub.Attributes["club"].Value), Convert.ToInt32(Sub.Attributes["selectable"].Value) == 1, Convert.ToString(Sub.InnerText)));
                    }
                }
            }
            XmlNodeList sets = xDoc.GetElementsByTagName("sets");
            foreach (XmlNode node in sets)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    this._setTypes.Add(childNode.Attributes["type"].Value, new FigureSet(SetTypeUtility.GetSetType(childNode.Attributes["type"].Value), Convert.ToInt32(childNode.Attributes["paletteid"].Value)));
                    foreach (XmlNode subChild in childNode.ChildNodes)
                    {
                        int id = 0;
                        if (int.TryParse(subChild.Attributes["id"].Value, out id))
                        {
                            if (!this._setTypes[childNode.Attributes["type"].Value].Sets.ContainsKey(id))
                            {
                                if (subChild.Attributes["selectable"] == null || subChild.Attributes["preselectable"] == null)
                                {
                                    continue;
                                }
                                this._setTypes[childNode.Attributes["type"].Value].Sets.Add(Convert.ToInt32(subChild.Attributes["id"].Value), new Set(Convert.ToInt32(subChild.Attributes["id"].Value), Convert.ToString(subChild.Attributes["gender"].Value), Convert.ToInt32(subChild.Attributes["club"].Value), Convert.ToInt32(subChild.Attributes["colorable"].Value) == 1, Convert.ToInt32(subChild.Attributes["selectable"].Value) == 1, Convert.ToInt32(subChild.Attributes["preselectable"].Value) == 1));
                                foreach (XmlNode grandChild in subChild.ChildNodes)
                                {
                                    if (grandChild.Attributes["type"] != null)
                                    {
                                        if (!this._setTypes[childNode.Attributes["type"].Value].Sets[id].Parts.ContainsKey(Convert.ToInt32(grandChild.Attributes["id"].Value) + "-" + grandChild.Attributes["type"].Value))
                                        {
                                            this._setTypes[childNode.Attributes["type"].Value].Sets[id].Parts.Add(Convert.ToInt32(grandChild.Attributes["id"].Value) + "-" + grandChild.Attributes["type"].Value,
                                      new Part(Convert.ToInt32(grandChild.Attributes["id"].Value), SetTypeUtility.GetSetType(childNode.Attributes["type"].Value), Convert.ToInt32(grandChild.Attributes["colorable"].Value) == 1, Convert.ToInt32(grandChild.Attributes["index"].Value), Convert.ToInt32(grandChild.Attributes["colorindex"].Value)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Faceless.
            this._setTypes["hd"].Sets.Add(99999, new Set(99999, "U", 0, true, false, false));
         //   log.Info("» Carregado " + this._palettes.Count + " cores de roupas!");
           // log.Info("» Carregado " + this._setTypes.Count + " roupas!");
        }
        public string ProcessFigure(string Figure, string gender, ICollection<ClothingParts> clothingParts, bool hasHabboClub)
        {
            return Figure; //filtering is done in UpdateFigure so no need to rebuild it here
        }
        public Palette GetPalette(int colorId)
        {
            return this._palettes.FirstOrDefault(x => x.Value.Colors.ContainsKey(colorId)).Value;
        }
        public bool TryGetPalette(int palletId, out Palette palette)
        {
            return this._palettes.TryGetValue(palletId, out palette);
        }
        public int GetRandomColor(int palletId)
        {
            return this._palettes[palletId].Colors.FirstOrDefault().Value.Id;
        }
    }
}