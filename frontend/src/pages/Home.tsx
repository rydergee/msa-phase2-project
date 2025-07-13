export default function Home() {
  return (
    <div className="max-w-6xl mx-auto">
      {/* Hero Section */}
      <div className="text-center mb-16">
        <div className="mb-8">
          <div className="inline-flex items-center justify-center w-20 h-20 bg-gradient-to-r from-orange-400 to-orange-500 rounded-full shadow-lg mb-6">
            <svg className="w-10 h-10 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
            </svg>
          </div>
        </div>
        <h1 className="text-5xl font-bold text-orange-800 mb-6">
          Welcome to MockMate
        </h1>
        <p className="text-xl text-orange-600 mb-10 max-w-3xl mx-auto leading-relaxed">
          Master Behavioral Interviews Through Peer-to-Peer Practice and Build Your Confidence with Real-World Experience
        </p>
        <div className="space-x-4">
          <a 
            href="/register" 
            className="inline-block bg-gradient-to-r from-orange-400 to-orange-500 text-white px-8 py-4 rounded-lg hover:from-orange-500 hover:to-orange-600 transition-all duration-200 font-semibold shadow-lg hover:shadow-xl transform hover:-translate-y-0.5"
          >
            Get Started
          </a>
          <a 
            href="/login" 
            className="inline-block bg-white text-orange-700 px-8 py-4 rounded-lg border-2 border-orange-200 hover:bg-orange-50 transition-all duration-200 font-semibold shadow-sm hover:shadow-md"
          >
            Login
          </a>
        </div>
      </div>

      {/* Core Features */}
      <div className="grid md:grid-cols-3 gap-8 mb-16">
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8 hover:shadow-xl transition-shadow duration-200">
          <div className="text-4xl mb-4">🎯</div>
          <h3 className="text-xl font-bold text-orange-800 mb-4">Mock Interview Sessions</h3>
          <p className="text-orange-600 leading-relaxed">
            Connect with peers for real-time behavioral interview practice with video chat and structured feedback.
          </p>
        </div>
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8 hover:shadow-xl transition-shadow duration-200">
          <div className="text-4xl mb-4">📝</div>
          <h3 className="text-xl font-bold text-orange-800 mb-4">STAR Method Journal</h3>
          <p className="text-orange-600 leading-relaxed">
            Document your experiences using the proven STAR method (Situation, Task, Action, Result) framework.
          </p>
        </div>
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8 hover:shadow-xl transition-shadow duration-200">
          <div className="text-4xl mb-4">📊</div>
          <h3 className="text-xl font-bold text-orange-800 mb-4">Progress Tracking</h3>
          <p className="text-orange-600 leading-relaxed">
            Receive detailed feedback and track your improvement across multiple interview sessions.
          </p>
        </div>
      </div>

      {/* How It Works */}
      <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8 mb-16">
        <h2 className="text-3xl font-bold text-orange-800 mb-8 text-center">How MockMate Works</h2>
        <div className="grid md:grid-cols-4 gap-8 text-center">
          <div className="space-y-4">
            <div className="bg-gradient-to-r from-orange-400 to-orange-500 rounded-full w-16 h-16 flex items-center justify-center mx-auto shadow-lg">
              <span className="text-white font-bold text-xl">1</span>
            </div>
            <h4 className="font-bold text-orange-800">Create Profile</h4>
            <p className="text-sm text-orange-600">Sign up and set up your university and study field</p>
          </div>
          <div className="space-y-4">
            <div className="bg-gradient-to-r from-orange-400 to-orange-500 rounded-full w-16 h-16 flex items-center justify-center mx-auto shadow-lg">
              <span className="text-white font-bold text-xl">2</span>
            </div>
            <h4 className="font-bold text-orange-800">Build Journal</h4>
            <p className="text-sm text-orange-600">Document experiences using STAR method</p>
          </div>
          <div className="space-y-4">
            <div className="bg-gradient-to-r from-orange-400 to-orange-500 rounded-full w-16 h-16 flex items-center justify-center mx-auto shadow-lg">
              <span className="text-white font-bold text-xl">3</span>
            </div>
            <h4 className="font-bold text-orange-800">Practice Sessions</h4>
            <p className="text-sm text-orange-600">Join video sessions with other students</p>
          </div>
          <div className="space-y-4">
            <div className="bg-gradient-to-r from-orange-400 to-orange-500 rounded-full w-16 h-16 flex items-center justify-center mx-auto shadow-lg">
              <span className="text-white font-bold text-xl">4</span>
            </div>
            <h4 className="font-bold text-orange-800">Get Feedback</h4>
            <p className="text-sm text-orange-600">Receive structured feedback and improve</p>
          </div>
        </div>
      </div>
    </div>
  );
}
