using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureChatChatMicroService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChatMicroService_Migration_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatParticipants_UserId",
                schema: "ChatMicroService",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                schema: "ChatMicroService",
                table: "Messages",
                column: "UserId",
                principalSchema: "ChatMicroService",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                schema: "ChatMicroService",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatParticipants_UserId",
                schema: "ChatMicroService",
                table: "Messages",
                column: "UserId",
                principalSchema: "ChatMicroService",
                principalTable: "ChatParticipants",
                principalColumn: "Id");
        }
    }
}
