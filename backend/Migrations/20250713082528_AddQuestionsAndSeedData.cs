using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MockMate.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionsAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Color = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false, defaultValue: "#3B82F6"),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SampleAnswer = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Tips = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    QuestionCategoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionCategories_QuestionCategoryId",
                        column: x => x.QuestionCategoryId,
                        principalTable: "QuestionCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InterviewSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ResponseTime = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSessions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "QuestionCategories",
                columns: new[] { "Id", "Color", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, "#8B5CF6", "Questions about leading teams, making decisions, and taking initiative", true, "Leadership", 1 },
                    { 2, "#10B981", "Questions about collaboration, communication, and working with others", true, "Teamwork", 2 },
                    { 3, "#F59E0B", "Questions about analytical thinking, creativity, and overcoming challenges", true, "Problem Solving", 3 },
                    { 4, "#EF4444", "Questions about managing disagreements and difficult situations", true, "Conflict Resolution", 4 },
                    { 5, "#3B82F6", "Questions about handling change, learning, and flexibility", true, "Adaptability", 5 },
                    { 6, "#EC4899", "Questions about accomplishments, goals, and delivering results", true, "Achievement", 6 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Category", "Difficulty", "IsActive", "QuestionCategoryId", "SampleAnswer", "Tags", "Text", "Tips" },
                values: new object[,]
                {
                    { 1, "Leadership", "Intermediate", true, null, "I was leading a software development team when our main client changed requirements halfway through the project. I called an emergency team meeting, reassigned tasks based on new priorities, and implemented daily check-ins to ensure we stayed on track. The project was delivered on time and the client was satisfied with the final product.", "leadership,team-management,project-management,decision-making", "Tell me about a time when you had to lead a team through a difficult project.", "Focus on your specific actions as a leader. Mention how you communicated with the team, made decisions, and ensured project success." },
                    { 2, "Leadership", "Advanced", true, null, "During my internship, our team leader was absent when a critical bug was discovered in production. I had to decide whether to roll back the deployment or attempt a quick fix. I gathered input from senior developers, assessed the risks, and chose to implement a targeted fix while preparing a rollback plan. The fix was successful and prevented major downtime.", "decision-making,leadership,problem-solving,risk-assessment", "Describe a situation where you had to make a difficult decision without all the information you needed.", "Emphasize your decision-making process, how you gathered available information, and the rationale behind your choice." },
                    { 3, "Leadership", "Advanced", true, null, "I needed to convince a colleague from another department to prioritize our shared project. I scheduled a meeting to understand their concerns, presented data showing the project's impact on both departments, and proposed a solution that would benefit their team as well. They agreed to prioritize the project and we successfully completed it together.", "influence,persuasion,cross-functional,collaboration", "Give me an example of when you had to influence someone without having authority over them.", "Show how you used persuasion skills, data, and mutual benefits rather than authority to influence others." },
                    { 4, "Teamwork", "Intermediate", true, null, "I was working on a group project with someone who was consistently late to meetings and missed deadlines. I scheduled a private conversation to understand their challenges and discovered they were struggling with time management. I offered to help them create a task breakdown and we established regular check-ins. Their performance improved significantly.", "teamwork,conflict-resolution,communication,mentoring", "Tell me about a time when you had to work with a difficult team member.", "Focus on how you approached the situation professionally and found constructive solutions rather than just complaining about the person." },
                    { 5, "Teamwork", "Beginner", true, null, "During a university hackathon, I worked with students from business, design, and engineering backgrounds. Initially, we struggled with communication as we all had different perspectives. I suggested we start by sharing our expertise and creating a common vocabulary. This helped us combine our diverse skills to create an innovative solution that won second place.", "collaboration,diversity,cross-functional,communication", "Describe a time when you had to collaborate with people from different backgrounds or departments.", "Highlight how you bridged differences and leveraged diverse perspectives as strengths rather than obstacles." },
                    { 6, "Problem Solving", "Intermediate", true, null, "Our e-commerce website was experiencing slow loading times during peak hours. I analyzed server logs, identified database queries as the bottleneck, and discovered several unoptimized queries. I implemented query optimization, added database indexing, and introduced caching. The page load time improved by 60% and customer satisfaction increased.", "problem-solving,analytical-thinking,technical-skills,optimization", "Tell me about a complex problem you solved and how you approached it.", "Walk through your systematic approach: problem identification, analysis, solution development, and results measurement." },
                    { 7, "Problem Solving", "Advanced", true, null, "Our team needed to reduce software licensing costs but couldn't compromise functionality. Instead of cutting features, I researched open-source alternatives and proposed a hybrid approach using both commercial and open-source tools. I created a migration plan and led the implementation, resulting in 40% cost savings while maintaining all required functionality.", "creativity,innovation,cost-reduction,strategic-thinking", "Describe a time when you had to think outside the box to solve a problem.", "Emphasize your creative thinking process and how your unconventional approach delivered better results than traditional solutions." },
                    { 8, "Conflict Resolution", "Advanced", true, null, "My manager wanted to rush a product release, but I believed it needed more testing. I prepared a presentation with data showing potential risks and customer impact. I proposed a compromise: release to a limited beta group first. My manager agreed, and we discovered critical bugs that would have affected thousands of users. The full release was successful after addressing these issues.", "conflict-resolution,communication,data-driven,advocacy", "Tell me about a time when you disagreed with your manager or supervisor.", "Show respect for authority while demonstrating your ability to advocate for what you believe is right using data and professional communication." },
                    { 9, "Conflict Resolution", "Intermediate", true, null, "Two team members disagreed about the technical approach for a project feature. I organized a meeting where each person presented their solution. I facilitated a discussion focusing on project requirements and asked clarifying questions. We identified that both approaches had merits and combined them into a hybrid solution that satisfied both team members and project needs.", "mediation,teamwork,technical-discussion,facilitation", "Describe a situation where you had to mediate between conflicting team members.", "Focus on your role as a neutral facilitator and how you helped the team find a collaborative solution." },
                    { 10, "Adaptability", "Beginner", true, null, "I was assigned to a project requiring knowledge of React, which I had never used before. I immediately started with online tutorials, built small practice projects, and sought mentorship from experienced React developers. Within two weeks, I was contributing effectively to the project and even suggested improvements to our component structure.", "learning-agility,adaptability,self-development,technology", "Tell me about a time when you had to quickly learn something new to complete a project.", "Demonstrate your learning agility and proactive approach to acquiring new skills under pressure." },
                    { 11, "Adaptability", "Intermediate", true, null, "I was working on a long-term research project when my manager asked me to immediately support a critical client issue. I quickly documented my current progress, created a transition plan, and shifted focus to the urgent issue. I solved the client problem within 24 hours and then efficiently resumed my research project without losing momentum.", "flexibility,prioritization,time-management,client-service", "Describe a time when priorities changed suddenly and how you handled it.", "Show how you can pivot quickly while maintaining organization and not losing sight of your original responsibilities." },
                    { 12, "Achievement", "Beginner", true, null, "During my internship, I automated a manual reporting process that was taking 8 hours per week. I analyzed the current workflow, designed a solution using Python scripts, and implemented automated data collection and report generation. This saved the team 32 hours per month and reduced errors by 95%. The solution is still being used today.", "achievement,automation,efficiency,impact,innovation", "Tell me about your greatest professional achievement.", "Choose an achievement that demonstrates measurable impact and showcases skills relevant to the role you're applying for." },
                    { 13, "Achievement", "Intermediate", true, null, "I set a goal to become a certified cloud solutions architect within six months while working full-time. I created a study schedule, joined online study groups, and built hands-on projects to practice. I passed the certification exam on my first attempt and immediately applied my new knowledge to optimize our company's cloud infrastructure, resulting in 25% cost savings.", "goal-setting,certification,self-improvement,cloud-computing", "Describe a goal you set for yourself and how you achieved it.", "Demonstrate your goal-setting process, commitment to self-improvement, and how you applied your achievement to create value." },
                    { 14, "Achievement", "Intermediate", true, null, "I was asked to create a simple customer feedback form, but I noticed an opportunity to implement a comprehensive feedback system. I designed a solution with real-time analytics, automated routing to relevant departments, and response tracking. The system increased customer satisfaction scores by 30% and is now used company-wide.", "exceeding-expectations,innovation,customer-satisfaction,systems-thinking", "Tell me about a time when you exceeded expectations.", "Show how you went beyond the minimum requirements and delivered additional value that had lasting impact." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_QuestionId",
                table: "InterviewSessions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_UserId",
                table: "InterviewSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_UserId_CreatedAt",
                table: "InterviewSessions",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_UserId_QuestionId",
                table: "InterviewSessions",
                columns: new[] { "UserId", "QuestionId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCategories_Name",
                table: "QuestionCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCategories_SortOrder",
                table: "QuestionCategories",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Category",
                table: "Questions",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Difficulty",
                table: "Questions",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_IsActive",
                table: "Questions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionCategoryId",
                table: "Questions",
                column: "QuestionCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterviewSessions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionCategories");
        }
    }
}
