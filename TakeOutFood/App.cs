namespace TakeOutFood
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security.Cryptography.X509Certificates;

    public class App
    {
        private IItemRepository itemRepository;
        private ISalesPromotionRepository salesPromotionRepository;

        public App(IItemRepository itemRepository, ISalesPromotionRepository salesPromotionRepository)
        {
            this.itemRepository = itemRepository;
            this.salesPromotionRepository = salesPromotionRepository;
        }

        public string BestCharge(List<string> inputs)
        {
            var sale = salesPromotionRepository.FindAll();
            var saleItems = sale[0].RelatedItems;
            var AllItems = itemRepository.FindAll();

            List<string[]> itemList = new List<string[]>();
            List<string[]> saledItemList = new List<string[]>();
            
            double totalPrice = 0;
            double savaPrice = 0;

            string result = "============= Order details =============\n";

            foreach (string orderString in inputs)
            {
                var itemId = orderString.Substring(0, 8);
                var itemNumber = orderString.Substring(11, 1);
                foreach (Item item in AllItems)
                {
                    if (item.Id == itemId)
                    {
                        var information = new string[4] { itemId, item.Name,itemNumber, item.Price.ToString() };
                        itemList.Add(information);
                        result += item.Name + " x " + itemNumber + " = " + (item.Price * int.Parse(itemNumber)).ToString()+" yuan\n";
                        totalPrice += item.Price * int.Parse(itemNumber);
                    }
                }
            }
            result += "-----------------------------------\n";

            foreach (string[] orderedItem in itemList)
            {
                foreach (string saledItem in saleItems)
                {
                    var itemSaledId =saledItem.Substring(0, 8);
                    if (itemSaledId == orderedItem[0])
                    {
                        saledItemList.Add(orderedItem);
                        savaPrice += int.Parse(orderedItem[2]) * int.Parse(orderedItem[3]) * 0.5;
                    }
                }
            }
            if (saledItemList.Count > 0)
            {
                result += "Promotion used:\nHalf price for certain dishes (";
                for (int k=0; k<saledItemList.Count;k++)
                {
                    if (k != 0)
                    {
                        result = result + ", " + saledItemList[k][1];
                    }
                    else
                    {
                        result += saledItemList[k][1];
                    }
                }
                result += "), saving ";
                result += savaPrice + " yuan\n-----------------------------------\n";
            }
            totalPrice -= savaPrice;
            result += "Total："+totalPrice+ " yuan\n===================================";

            return result;
        }
    }
}
