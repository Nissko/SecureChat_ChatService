using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Enums
{
    public class TypeOfMessageEnum : Enumeration
    {
        public TypeOfMessageEnum(Guid id, string name) : base(id, name)
        { }

        public static IEnumerable<TypeOfMessageEnum> List()
        {
            return
            [
                Text,
                Picture
            ];
        }

        public static TypeOfMessageEnum FromName(string typeOfCourseFromName)
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
    
        public static TypeOfMessageEnum FromId(Guid fieldTypeId)
        {
            var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

                throw new(typeOfCourseIsExists);
            }
        }

        public static readonly TypeOfMessageEnum Text = new(
            Guid.Parse("d05131f2-f76e-432b-b73e-ca3678c397b7"),
            "Текст".ToLowerInvariant());

        public static readonly TypeOfMessageEnum Picture = new(
            Guid.Parse("0bb137e9-d604-4ee8-8eb2-bcf97ac7dfb9"),
            "Изображение".ToLowerInvariant());
    }
}