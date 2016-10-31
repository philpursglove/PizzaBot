using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace PizzaBot
{
    public enum Crust
    {
        ThinAndCrispy = 1,
        DeepPan,
        StuffedCrust
    }

    public enum Topping
    {
        CheeseAndTomato = 1,
        AmericanHot,
        MightyMeaty
    }

    public enum Sides
    {
        ChickenWings = 1,
        GarlicBread,
        DoughBalls
    }

    [Serializable]
    public class PizzaOrder
    {
        public Crust Crust { get; set; }
        public Topping Topping { get; set; }
        
        public List<Sides> Sides { get; set; }

        public static IForm<PizzaOrder> BuildForm()
        {
            return new FormBuilder<PizzaOrder>()
                .Message("I am PizzaBot")
                .Build();
        }
    }
}