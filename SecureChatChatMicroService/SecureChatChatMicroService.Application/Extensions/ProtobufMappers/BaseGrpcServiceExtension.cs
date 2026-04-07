using ChatMicroService.Common;
using Dtos.DTO.ChatDtos.Crud;
using Dtos.DTO.ChatGroupDtos.Crud;
using Dtos.DTO.ChatParticipantsDtos.Crud;
using Dtos.DTO.GroupDtos.Crud;
using Dtos.DTO.MessageDtos.Crud;
using Dtos.DTO.UserDtos.Crud;
using Google.Protobuf.WellKnownTypes;

namespace SecureChatChatMicroService.Application.Extensions.ProtobufMappers
{
    public static class BaseGrpcServiceExtension
    {
        #region User

        public static UserInfo ToProtoUserInfo(this UserDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                UserProfileId = dto.UserProfileId.ToString(),
                IsDeleted = dto.IsDeleted,
                Groups = { dto.Groups.ToList().ToProtoGroupInfoList() }
            };
        }

        public static List<UserInfo> ToProtoUserInfoList(
            this List<UserDto> dtos)
        {
            return dtos.Select(ToProtoUserInfo).ToList();
        }

        #endregion

        #region Group

        public static GroupInfo ToProtoGroupInfo(this GroupDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                UserId = dto.UserId.ToString(),
                ChatGroups = { dto.ChatGroups.ToList().ToProtoChatGroupInfoList() }
            };
        }

        public static List<GroupInfo> ToProtoGroupInfoList(
            this List<GroupDto> dtos)
        {
            return dtos.Select(ToProtoGroupInfo).ToList();
        }

        #endregion

        #region ChatGroup

        public static ChatGroupInfo ToProtoChatGroupInfo(this ChatGroupDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                ChatId = dto.ChatId.ToString(),
                GroupId = dto.GroupId.ToString()
            };
        }

        public static List<ChatGroupInfo> ToProtoChatGroupInfoList(
            this List<ChatGroupDto> dtos)
        {
            return dtos.Select(ToProtoChatGroupInfo).ToList();
        }

        #endregion

        #region Chat

        public static ChatInfo ToProtoChatInfo(this ChatDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                LastMessageTime = Timestamp.FromDateTime(dto.LastMessageTime.ToDateTimeUtc()),
                CountUnreadMessages = dto.CountUnreadMessages,
                IsPint = dto.IsPint,
                IsMute = dto.IsMute,
                IsDeleted = dto.IsDeleted,
                Type = dto.Type.ToString(),
                OwnerId = dto.OwnerId.ToString(),
                ChatGroups = {dto.ChatGroups.ToList().ToProtoChatGroupInfoList()},
                ChatParticipants = { dto.ChatParticipants.ToList().ToProtoChatParticipantsInfoList() },
                Messages = { dto.Messages.ToList().ToProtoMessageInfoList() }
            };
        }

        public static List<ChatInfo> ToProtoChatInfoList(
            this List<ChatDto> dtos)
        {
            return dtos.Select(ToProtoChatInfo).ToList();
        }

        #endregion

        #region ChatParticipant

        public static ChatParticipantsInfo ToProtoChatParticipantsInfo(this ChatParticipantsDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                EnterTime = Timestamp.FromDateTime(dto.EnterTime.ToDateTimeUtc()),
                ExitTime = Timestamp.FromDateTime(dto.ExitTime!.Value.ToDateTimeUtc()),
                ChatId = dto.ChatId.ToString(),
                UserId = dto.UserId.ToString()
            };
        }

        public static List<ChatParticipantsInfo> ToProtoChatParticipantsInfoList(
            this List<ChatParticipantsDto> dtos)
        {
            return dtos.Select(ToProtoChatParticipantsInfo).ToList();
        }

        #endregion

        #region Message

        public static MessageInfo ToProtoMessageInfo(this MessageDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                AnswerMessageId = dto.AnswerMessageId.ToString(),
                ChatId = dto.ChatId.ToString(),
                UserId = dto.UserId.ToString(),
                TypeOfMessage = dto.TypeOfMessage.ToString(),
                Content = dto.Content,
                SendTime = Timestamp.FromDateTime(dto.SendTime.ToDateTimeUtc()),
                UpdateTime = Timestamp.FromDateTime(dto.UpdateTime!.Value.ToDateTimeUtc()),
                DeleteTime = Timestamp.FromDateTime(dto.DeleteTime!.Value.ToDateTimeUtc()),
                IsEdited = dto.IsEdited,
                IsDeleted = dto.IsDeleted,
            };
        }

        public static List<MessageInfo> ToProtoMessageInfoList(
            this List<MessageDto> dtos)
        {
            return dtos.Select(ToProtoMessageInfo).ToList();
        }

        #endregion
    }
}