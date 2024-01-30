using CourierConnect.DataAccess.Data;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace IntegrationTests.Helpers
{
    public static class Utilities
    {
        // <snippet1>
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            InitializeDbForTests(db);
        }

        //public static List<Message> GetSeedingMessages()
        //{
        //    return new List<Message>()
        //{
        //    new Message(){ Text = "TEST RECORD: You're standing on my scarf." },
        //    new Message(){ Text = "TEST RECORD: Would you like a jelly baby?" },
        //    new Message(){ Text = "TEST RECORD: To the rational mind, " +
        //        "nothing is inexplicable; only unexplained." }
        //};
        //}
        // </snippet1>
    }
}
