using System.Collections.Generic;
using System.Linq;

namespace Despegar.Core.Neo.API
{
    /// <summary>
    /// Mocks repository
    /// </summary>
    public class Mock
    {
        public string MockName { get; set; }
        public ServiceKey ServiceID { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// Contains all the available mocks
        /// </summary>
        private static readonly List<Mock> mocksRepo = new List<Mock>();

        public static Mock GetMock(string key)
        {
            return mocksRepo.First(x => x.MockName == key);
        }

        public static List<Mock> AllMocks { get { return mocksRepo; } }      

        public static void AddMockToRepo(Mock mock)
        {
            mocksRepo.Add(mock);
        }
    }
}