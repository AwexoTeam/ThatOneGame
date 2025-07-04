using Gum.Converters;
using Gum.DataTypes;
using Gum.DataTypes.Variables;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using RpgGame.Managers;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.UI
{
    public static class PlayerStatView
    {
        public static Dictionary<string, Stats[]> statCategories = new Dictionary<string, Stats[]>()
        {
            { 
                "Vitals", new Stats[]
                { 
                    Stats.Hp,
                    Stats.HP_Leech,
                    Stats.Health_Regen,
                    Stats.Wound,

                    Stats.Mp,
                    Stats.MP_Leech,
                    Stats.Mana_Regen,

                    Stats.Sp,
                    Stats.SP_Leech,
                    Stats.Special_Regen,

                } 
            },

            {
                "Major Stats", new Stats[]
                {
                    Stats.Stregnth,
                    Stats.Intelligence,
                    Stats.Dexterity,

                    Stats.Will,
                    Stats.Luck,
                }
            },

            {
                "Damage", new Stats[]
                {
                    Stats.Magic_Damage,

                    Stats.Min_Damage,
                    Stats.Min_Injury,

                    Stats.Balance,
                    Stats.Critical_Chance,
                    Stats.Critical_Damage,

                }
            },

            {
                "Defence", new Stats[]
                {
                    Stats.Defence,

                    Stats.Damage_Reduction,
                    Stats.Armor_Pierce,


                }
            },
        };

        public static ColoredRectangleRuntime playerPortrait;

        public static ColoredRectangleRuntime statContainer;
        public static ColoredRectangleRuntime statBlockContainer;
        public static ColoredRectangleRuntime statControlContainer;
        
        public static ColoredRectangleRuntime statNameBarContainer;
        public static TextRuntime statBarNames;

        public static ColoredRectangleRuntime statValueBarContainer;
        public static TextRuntime statBarText;

        public static TextRuntime statCategoryText;

        private static int currentCategory = 0;

        public static void Initialize()
        {
            EventManager.EntityStatChanged += EntityStatChanged;
        }


        private static void EntityStatChanged(Entity entity, Stats stat, float oldValue, float newValue)
        {
            if (entity != Player.instance.entity)
                return;

            var category = statCategories.ElementAt(currentCategory);
            if (!category.Value.Contains(stat))
                return;

            UpdateUI();
        }

        private static void UpdateUI()
        {
            UpdateStatBlocks();
        }

        public static GraphicalUiElement GetStatView()
        {

            var container = new ContainerRuntime();
            container.HeightUnits = DimensionUnitType.PercentageOfParent;
            container.WidthUnits = DimensionUnitType.PercentageOfParent;
            container.Height = 100;
            container.Width = 100;

            SetupPlayerPortrait();
            SetupStatBar();

            container.AddChild(statContainer);
            container.AddChild(playerPortrait);

            return container;
        }

        private static void SetupStatBar()
        {
            statContainer = new ColoredRectangleRuntime();

            statContainer.WidthUnits = DimensionUnitType.PercentageOfParent;
            statContainer.HeightUnits = DimensionUnitType.PercentageOfParent;
            statContainer.Width = 98;
            statContainer.XUnits = GeneralUnitType.Percentage;
            statContainer.X = 2;
            statContainer.Anchor(Anchor.Bottom);
            statContainer.Y = 10;
            statContainer.Alpha = 0;

            statBlockContainer = new ColoredRectangleRuntime();
            statContainer.AddChild(statBlockContainer);
            
            statBlockContainer.WidthUnits = DimensionUnitType.PercentageOfParent;
            statBlockContainer.HeightUnits = DimensionUnitType.PercentageOfParent;
            statBlockContainer.Height = 90;
            statBlockContainer.Width = 100;
            statBlockContainer.Anchor(Anchor.Bottom);
            statBlockContainer.Color = Color.Green;
            statBlockContainer.Alpha = 0;

            statNameBarContainer = new ColoredRectangleRuntime();
            statValueBarContainer = new ColoredRectangleRuntime();

            statBlockContainer.AddChild(statNameBarContainer);
            statBlockContainer.AddChild(statValueBarContainer);

            statNameBarContainer.HeightUnits = DimensionUnitType.PercentageOfParent;
            statNameBarContainer.WidthUnits = DimensionUnitType.PercentageOfParent;
            statNameBarContainer.Color = Color.LightBlue;
            statNameBarContainer.Height = 100;
            statNameBarContainer.Width = 70;
            statNameBarContainer.Alpha = 0;

            statValueBarContainer.WidthUnits = DimensionUnitType.PercentageOfParent;
            statValueBarContainer.HeightUnits = DimensionUnitType.PercentageOfParent;
            statValueBarContainer.Anchor(Anchor.Right);
            statValueBarContainer.Color = Color.Gray;
            statValueBarContainer.Height = 100;
            statValueBarContainer.Width = 30;
            
            SetupStatText(ref statBarNames);
            SetupStatText(ref statBarText);

            statControlContainer = new ColoredRectangleRuntime();
            statContainer.AddChild(statControlContainer);
            statControlContainer.Color = Color.Black;

            statControlContainer.WidthUnits = DimensionUnitType.PercentageOfParent;
            statControlContainer.HeightUnits = DimensionUnitType.PercentageOfParent;
            statControlContainer.Height = 10;
            statControlContainer.Width = 100;

            statNameBarContainer.AddChild(statBarNames);
            statValueBarContainer.AddChild(statBarText);

            statCategoryText = new TextRuntime();
            statControlContainer.AddChild(statCategoryText);
            statCategoryText.WidthUnits = DimensionUnitType.PercentageOfParent;
            statCategoryText.HeightUnits = DimensionUnitType.PercentageOfParent;
            statCategoryText.Height = 100;
            statCategoryText.Width = 100;

            statCategoryText.HorizontalAlignment = HorizontalAlignment.Center;
            statCategoryText.FontSize = 16;
            statCategoryText.Color = Color.White;

            UpdateStatBlocks();
        }

        private static void UpdateStatBlocks()
        {
            var list = statCategories.ElementAt(currentCategory);

            statBarNames.Text = "";
            statBarText.Text = "";

            foreach (var stat in list.Value)
            {
                int id = (int)stat;
                if (id < 3 || id == 10)
                    continue;

                statBarNames.Text += GetStatName(stat);
                statBarText.Text += GetStatValue(stat);
            }

            for (int i = 0; i < 2; i++)
            {
                SetupButton(i, statControlContainer);
            }

            statCategoryText.Text = list.Key;
        }

        private static void SetupButton(int i, ColoredRectangleRuntime parent)
        {
            Anchor anchor = i == 0 ? Anchor.Left : Anchor.Right;
            string text = i == 0 ? "<" : ">";

            Button btn = new Button();
            parent.AddChild(btn.Visual);

            btn.Text = text;
            btn.Anchor(anchor);

            btn.Visual.WidthUnits = DimensionUnitType.PercentageOfOtherDimension;
            btn.Visual.HeightUnits = DimensionUnitType.PercentageOfParent;

            btn.Width = 100;
            btn.Height = 100;

            btn.Click += Btn_Click;
        }

        private static void Btn_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int val = btn.Text == ">" ? 1 : -1;

            currentCategory += val;
            if (currentCategory < 0)
                currentCategory = statCategories.Count - 1;

            if (currentCategory == statCategories.Count - 1)
                currentCategory = 0;

            UpdateStatBlocks();
        }

        private static string GetStatValue(Stats stat)
        {
            int id = (int)stat;
            if (id < 3 && id < 10)
                return GetVitalText(stat);

            var entity = Player.instance.entity;

            string rtn = entity.GetStat(stat).ToString();
            
            if (stat == Stats.Min_Damage)
                rtn += "-" + entity.GetStat(Stats.Max_Damage);

            if(stat == Stats.Min_Injury)
                rtn += "-" + entity.GetStat(Stats.Max_Injury);

            if (stat == Stats.Critical_Chance || stat == Stats.Critical_Damage || stat == Stats.Balance || stat == Stats.Min_Injury)
                rtn += "%";

            return rtn + "\n";
        }

        private static void SetupStatText(ref TextRuntime statBar)
        {
            statBar = new TextRuntime();
            statBar.WidthUnits = DimensionUnitType.PercentageOfParent;
            statBar.HeightUnits = DimensionUnitType.PercentageOfParent;
            statBar.Height = 100;
            statBar.Width = 100;

            statBar.Text = "";
            statBar.Color = Color.Black;
            statBar.HorizontalAlignment = HorizontalAlignment.Left;

            statBar.FontSize = 10;
        }

        private static string GetStatName(Stats stat)
        {
            int id = (int)stat;
            
            string rtn = stat.ToString();
            if (stat == Stats.Min_Injury)
                rtn = "Injury Rate";

            if (stat == Stats.Min_Damage)
                rtn = "Physical Damage";

            rtn = rtn.Replace("_", " ");
            
            rtn += ": ";
            
            rtn += "\n";

            return rtn;
        }

        private static string GetVitalText(Stats stat)
        {
            int id = (int)stat;
            if (id > 3 && id != 10)
                return string.Empty;

            var entity = Player.instance.entity;

            string text = "{0}/{1} \n";
            string name = stat.ToString().Replace("_", " ");

            string val = entity.GetStat(stat).ToString(); ;

            Stats currValue = Stats.Wound;
            currValue = stat == Stats.Max_Hp ? Stats.Hp : currValue;
            currValue = stat == Stats.Max_Mp ? Stats.Mp : currValue;
            currValue = stat == Stats.Max_Sp ? Stats.Sp : currValue;

            float curr = entity.GetStat(currValue);

            return String.Format(text, curr, val);
        }

        public static void SetupPlayerPortrait()
        {
            playerPortrait = new ColoredRectangleRuntime();
            playerPortrait.WidthUnits = DimensionUnitType.PercentageOfParent;
            playerPortrait.Width = 80;
            playerPortrait.XUnits = GeneralUnitType.Percentage;
            playerPortrait.X = 10;

            playerPortrait.HeightUnits = DimensionUnitType.PercentageOfOtherDimension;
            playerPortrait.Height = 110;
            playerPortrait.YUnits = GeneralUnitType.Percentage;
            playerPortrait.Y = 2;

        }
    }
}
