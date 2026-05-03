using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureChatChatMicroService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChatMicroService_Migration_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatParticipants_ChatParticipantsId",
                schema: "ChatMicroService",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ChatParticipantsId",
                schema: "ChatMicroService",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatParticipantsId",
                schema: "ChatMicroService",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatParticipants_UserId",
                schema: "ChatMicroService",
                table: "Messages",
                column: "UserId",
                principalSchema: "ChatMicroService",
                principalTable: "ChatParticipants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatParticipants_UserId",
                schema: "ChatMicroService",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "ChatMicroService",
                table: "Messages",
                newName: "ChatParticipantsId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                schema: "ChatMicroService",
                table: "Messages",
                newName: "IX_Messages_ChatParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatParticipants_ChatParticipantsId",
                schema: "ChatMicroService",
                table: "Messages",
                column: "ChatParticipantsId",
                principalSchema: "ChatMicroService",
                principalTable: "ChatParticipants",
                principalColumn: "Id");
        }
    }
}
