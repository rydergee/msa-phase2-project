using MockMate.Api.Models;

namespace MockMate.Api.Data;

public static class SeedData
{
    public static class Questions
    {
        public static readonly List<QuestionCategory> Categories = new()
        {
            new QuestionCategory
            {
                Id = 1,
                Name = "Leadership",
                Description = "Questions about leading teams, making decisions, and taking initiative",
                Color = "#8B5CF6",
                SortOrder = 1
            },
            new QuestionCategory
            {
                Id = 2,
                Name = "Teamwork",
                Description = "Questions about collaboration, communication, and working with others",
                Color = "#10B981",
                SortOrder = 2
            },
            new QuestionCategory
            {
                Id = 3,
                Name = "Problem Solving",
                Description = "Questions about analytical thinking, creativity, and overcoming challenges",
                Color = "#F59E0B",
                SortOrder = 3
            },
            new QuestionCategory
            {
                Id = 4,
                Name = "Conflict Resolution",
                Description = "Questions about managing disagreements and difficult situations",
                Color = "#EF4444",
                SortOrder = 4
            },
            new QuestionCategory
            {
                Id = 5,
                Name = "Adaptability",
                Description = "Questions about handling change, learning, and flexibility",
                Color = "#3B82F6",
                SortOrder = 5
            },
            new QuestionCategory
            {
                Id = 6,
                Name = "Achievement",
                Description = "Questions about accomplishments, goals, and delivering results",
                Color = "#EC4899",
                SortOrder = 6
            }
        };

        public static readonly List<Question> BehavioralQuestions = new()
        {
            // Leadership Questions
            new Question
            {
                Id = 1,
                Text = "Tell me about a time when you had to lead a team through a difficult project.",
                Category = "Leadership",
                Difficulty = "Intermediate",
                SampleAnswer = "I was leading a software development team when our main client changed requirements halfway through the project. I called an emergency team meeting, reassigned tasks based on new priorities, and implemented daily check-ins to ensure we stayed on track. The project was delivered on time and the client was satisfied with the final product.",
                Tips = "Focus on your specific actions as a leader. Mention how you communicated with the team, made decisions, and ensured project success.",
                Tags = "leadership,team-management,project-management,decision-making"
            },
            new Question
            {
                Id = 2,
                Text = "Describe a situation where you had to make a difficult decision without all the information you needed.",
                Category = "Leadership",
                Difficulty = "Advanced",
                SampleAnswer = "During my internship, our team leader was absent when a critical bug was discovered in production. I had to decide whether to roll back the deployment or attempt a quick fix. I gathered input from senior developers, assessed the risks, and chose to implement a targeted fix while preparing a rollback plan. The fix was successful and prevented major downtime.",
                Tips = "Emphasize your decision-making process, how you gathered available information, and the rationale behind your choice.",
                Tags = "decision-making,leadership,problem-solving,risk-assessment"
            },
            new Question
            {
                Id = 3,
                Text = "Give me an example of when you had to influence someone without having authority over them.",
                Category = "Leadership",
                Difficulty = "Advanced",
                SampleAnswer = "I needed to convince a colleague from another department to prioritize our shared project. I scheduled a meeting to understand their concerns, presented data showing the project's impact on both departments, and proposed a solution that would benefit their team as well. They agreed to prioritize the project and we successfully completed it together.",
                Tips = "Show how you used persuasion skills, data, and mutual benefits rather than authority to influence others.",
                Tags = "influence,persuasion,cross-functional,collaboration"
            },

            // Teamwork Questions
            new Question
            {
                Id = 4,
                Text = "Tell me about a time when you had to work with a difficult team member.",
                Category = "Teamwork",
                Difficulty = "Intermediate",
                SampleAnswer = "I was working on a group project with someone who was consistently late to meetings and missed deadlines. I scheduled a private conversation to understand their challenges and discovered they were struggling with time management. I offered to help them create a task breakdown and we established regular check-ins. Their performance improved significantly.",
                Tips = "Focus on how you approached the situation professionally and found constructive solutions rather than just complaining about the person.",
                Tags = "teamwork,conflict-resolution,communication,mentoring"
            },
            new Question
            {
                Id = 5,
                Text = "Describe a time when you had to collaborate with people from different backgrounds or departments.",
                Category = "Teamwork",
                Difficulty = "Beginner",
                SampleAnswer = "During a university hackathon, I worked with students from business, design, and engineering backgrounds. Initially, we struggled with communication as we all had different perspectives. I suggested we start by sharing our expertise and creating a common vocabulary. This helped us combine our diverse skills to create an innovative solution that won second place.",
                Tips = "Highlight how you bridged differences and leveraged diverse perspectives as strengths rather than obstacles.",
                Tags = "collaboration,diversity,cross-functional,communication"
            },

            // Problem Solving Questions
            new Question
            {
                Id = 6,
                Text = "Tell me about a complex problem you solved and how you approached it.",
                Category = "Problem Solving",
                Difficulty = "Intermediate",
                SampleAnswer = "Our e-commerce website was experiencing slow loading times during peak hours. I analyzed server logs, identified database queries as the bottleneck, and discovered several unoptimized queries. I implemented query optimization, added database indexing, and introduced caching. The page load time improved by 60% and customer satisfaction increased.",
                Tips = "Walk through your systematic approach: problem identification, analysis, solution development, and results measurement.",
                Tags = "problem-solving,analytical-thinking,technical-skills,optimization"
            },
            new Question
            {
                Id = 7,
                Text = "Describe a time when you had to think outside the box to solve a problem.",
                Category = "Problem Solving",
                Difficulty = "Advanced",
                SampleAnswer = "Our team needed to reduce software licensing costs but couldn't compromise functionality. Instead of cutting features, I researched open-source alternatives and proposed a hybrid approach using both commercial and open-source tools. I created a migration plan and led the implementation, resulting in 40% cost savings while maintaining all required functionality.",
                Tips = "Emphasize your creative thinking process and how your unconventional approach delivered better results than traditional solutions.",
                Tags = "creativity,innovation,cost-reduction,strategic-thinking"
            },

            // Conflict Resolution Questions
            new Question
            {
                Id = 8,
                Text = "Tell me about a time when you disagreed with your manager or supervisor.",
                Category = "Conflict Resolution",
                Difficulty = "Advanced",
                SampleAnswer = "My manager wanted to rush a product release, but I believed it needed more testing. I prepared a presentation with data showing potential risks and customer impact. I proposed a compromise: release to a limited beta group first. My manager agreed, and we discovered critical bugs that would have affected thousands of users. The full release was successful after addressing these issues.",
                Tips = "Show respect for authority while demonstrating your ability to advocate for what you believe is right using data and professional communication.",
                Tags = "conflict-resolution,communication,data-driven,advocacy"
            },
            new Question
            {
                Id = 9,
                Text = "Describe a situation where you had to mediate between conflicting team members.",
                Category = "Conflict Resolution",
                Difficulty = "Intermediate",
                SampleAnswer = "Two team members disagreed about the technical approach for a project feature. I organized a meeting where each person presented their solution. I facilitated a discussion focusing on project requirements and asked clarifying questions. We identified that both approaches had merits and combined them into a hybrid solution that satisfied both team members and project needs.",
                Tips = "Focus on your role as a neutral facilitator and how you helped the team find a collaborative solution.",
                Tags = "mediation,teamwork,technical-discussion,facilitation"
            },

            // Adaptability Questions
            new Question
            {
                Id = 10,
                Text = "Tell me about a time when you had to quickly learn something new to complete a project.",
                Category = "Adaptability",
                Difficulty = "Beginner",
                SampleAnswer = "I was assigned to a project requiring knowledge of React, which I had never used before. I immediately started with online tutorials, built small practice projects, and sought mentorship from experienced React developers. Within two weeks, I was contributing effectively to the project and even suggested improvements to our component structure.",
                Tips = "Demonstrate your learning agility and proactive approach to acquiring new skills under pressure.",
                Tags = "learning-agility,adaptability,self-development,technology"
            },
            new Question
            {
                Id = 11,
                Text = "Describe a time when priorities changed suddenly and how you handled it.",
                Category = "Adaptability",
                Difficulty = "Intermediate",
                SampleAnswer = "I was working on a long-term research project when my manager asked me to immediately support a critical client issue. I quickly documented my current progress, created a transition plan, and shifted focus to the urgent issue. I solved the client problem within 24 hours and then efficiently resumed my research project without losing momentum.",
                Tips = "Show how you can pivot quickly while maintaining organization and not losing sight of your original responsibilities.",
                Tags = "flexibility,prioritization,time-management,client-service"
            },

            // Achievement Questions
            new Question
            {
                Id = 12,
                Text = "Tell me about your greatest professional achievement.",
                Category = "Achievement",
                Difficulty = "Beginner",
                SampleAnswer = "During my internship, I automated a manual reporting process that was taking 8 hours per week. I analyzed the current workflow, designed a solution using Python scripts, and implemented automated data collection and report generation. This saved the team 32 hours per month and reduced errors by 95%. The solution is still being used today.",
                Tips = "Choose an achievement that demonstrates measurable impact and showcases skills relevant to the role you're applying for.",
                Tags = "achievement,automation,efficiency,impact,innovation"
            },
            new Question
            {
                Id = 13,
                Text = "Describe a goal you set for yourself and how you achieved it.",
                Category = "Achievement",
                Difficulty = "Intermediate",
                SampleAnswer = "I set a goal to become a certified cloud solutions architect within six months while working full-time. I created a study schedule, joined online study groups, and built hands-on projects to practice. I passed the certification exam on my first attempt and immediately applied my new knowledge to optimize our company's cloud infrastructure, resulting in 25% cost savings.",
                Tips = "Demonstrate your goal-setting process, commitment to self-improvement, and how you applied your achievement to create value.",
                Tags = "goal-setting,certification,self-improvement,cloud-computing"
            },
            new Question
            {
                Id = 14,
                Text = "Tell me about a time when you exceeded expectations.",
                Category = "Achievement",
                Difficulty = "Intermediate",
                SampleAnswer = "I was asked to create a simple customer feedback form, but I noticed an opportunity to implement a comprehensive feedback system. I designed a solution with real-time analytics, automated routing to relevant departments, and response tracking. The system increased customer satisfaction scores by 30% and is now used company-wide.",
                Tips = "Show how you went beyond the minimum requirements and delivered additional value that had lasting impact.",
                Tags = "exceeding-expectations,innovation,customer-satisfaction,systems-thinking"
            }
        };
    }
}
