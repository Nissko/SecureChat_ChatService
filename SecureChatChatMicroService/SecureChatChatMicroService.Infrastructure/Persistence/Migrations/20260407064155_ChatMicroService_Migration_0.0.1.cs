using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace SecureChatChatMicroService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChatMicroService_Migration_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ChatMicroService");

            migrationBuilder.CreateTable(
                name: "Chats",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastMessageTime = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата последнего сообщения"),
                    CountUnreadMessages = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Кол-во непрочитанных сообщений"),
                    IsPint = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Закреплен ли чат"),
                    IsMute = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Показывать ли уведомления"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Удален ли"),
                    Type = table.Column<Guid>(type: "uuid", nullable: false, comment: "Тип (чат, канал, группа)"),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Создатель чата")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatTypes",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Тип чата")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfMessages",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Тип сообщения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Ид профиля пользователя"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Удален ли")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatParticipants",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EnterTime = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата входа"),
                    ExitTime = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата выхода"),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatParticipants_Chats_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Chats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Название"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SendTime = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата отправки"),
                    UpdateTime = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    DeleteTime = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    Content = table.Column<string>(type: "text", nullable: false, comment: "Текст сообщения"),
                    TypeOfMessage = table.Column<Guid>(type: "uuid", nullable: false, comment: "Тип сообщения"),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Изменено ли"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Удалено ли")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Chats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Messages_AnswerMessageId",
                        column: x => x.AnswerMessageId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Messages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatGroup",
                schema: "ChatMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Ид чата"),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Ид группы")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatGroup_Chats_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatGroup_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "ChatMicroService",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroup_ChatId",
                schema: "ChatMicroService",
                table: "ChatGroup",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroup_GroupId",
                schema: "ChatMicroService",
                table: "ChatGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatParticipants_ChatId",
                schema: "ChatMicroService",
                table: "ChatParticipants",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatParticipants_UserId",
                schema: "ChatMicroService",
                table: "ChatParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatTypes_Id",
                schema: "ChatMicroService",
                table: "ChatTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                schema: "ChatMicroService",
                table: "Groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AnswerMessageId",
                schema: "ChatMicroService",
                table: "Messages",
                column: "AnswerMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                schema: "ChatMicroService",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                schema: "ChatMicroService",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfMessages_Id",
                schema: "ChatMicroService",
                table: "TypeOfMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatGroup",
                schema: "ChatMicroService");

            migrationBuilder.DropTable(
                name: "ChatParticipants",
                schema: "ChatMicroService");

            migrationBuilder.DropTable(
                name: "ChatTypes",
                schema: "ChatMicroService");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "ChatMicroService");

            migrationBuilder.DropTable(
                name: "TypeOfMessages",
                schema: "ChatMicroService");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "ChatMicroService");

            migrationBuilder.DropTable(
                name: "Chats",
                schema: "ChatMicroService");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "ChatMicroService");
        }
    }
}
