using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Enums
{
    public class ChatTypeEnum : Enumeration
    {
        public ChatTypeEnum(Guid id, string name) : base(id, name)
        { }

        public static IEnumerable<ChatTypeEnum> List()
        {
            return
            [
                ChatType,
                ChannelType,
                GroupType
            ];
        }

        public static ChatTypeEnum FromName(string typeOfCourseFromName)
        {
            var request = List()
                .SingleOrDefault(s =>
                    string.Equals(s.Name, typeOfCourseFromName, StringComparison.CurrentCultureIgnoreCase));

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Name));

                /*TODO: Кастомное исключение*/
                throw new ArgumentNullException(typeOfCourseIsExists);
            }
        }
    
        public static ChatTypeEnum FromId(Guid fieldTypeId)
        {
            var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

                throw new(typeOfCourseIsExists);
            }
        }

        public static readonly ChatTypeEnum ChatType = new(
            Guid.Parse("b0ff82cc-5088-45d2-8517-92b051bf60a5"),
            "Чат".ToLowerInvariant());

        public static readonly ChatTypeEnum ChannelType = new(
            Guid.Parse("33305e0a-9635-42f9-8315-877c093f8c1b"),
            "Канал".ToLowerInvariant());

        public static readonly ChatTypeEnum GroupType = new(
            Guid.Parse("422975b0-d529-4638-8dc6-3bc99cc16904"),
            "Группа".ToLowerInvariant());
    }
}