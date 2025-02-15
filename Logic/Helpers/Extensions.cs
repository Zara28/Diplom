using System.Text;

namespace OfficeTime.Logic.Helpers
{
    public static class Extensions
    {
        public static int? GetId(this ISession session)
        {
            var value = session.Get("session");

            if (value == null)
            {
                return null;
            }

            var sessionId = Encoding.ASCII.GetString(value);

            if (Int32.TryParse(sessionId, out var id))
            {
                return id;
            }
            else return null;
        }
    }
}
