export default function Home() {
  return (
    <div className="max-w-4xl mx-auto">
      <div className="text-center mb-8">
        <h1 className="text-4xl font-bold text-gray-900 mb-4">
          Welcome to MockMate
        </h1>
        <p className="text-xl text-gray-600 mb-8">
          Master Behavioral Interviews Through Peer-to-Peer Practice
        </p>
        <div className="space-x-4">
          <a 
            href="/register" 
            className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors"
          >
            Get Started
          </a>
          <a 
            href="/login" 
            className="bg-gray-200 text-gray-800 px-6 py-3 rounded-lg hover:bg-gray-300 transition-colors"
          >
            Login
          </a>
        </div>
      </div>

      {/* Core Features */}
      <div className="grid md:grid-cols-3 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow-md p-6">
          <h3 className="text-xl font-bold mb-3">🎯 Mock Interview Sessions</h3>
          <p className="text-gray-600">
            Connect with peers for real-time behavioral interview practice with video chat and structured feedback.
          </p>
        </div>
        <div className="bg-white rounded-lg shadow-md p-6">
          <h3 className="text-xl font-bold mb-3">📝 STAR Method Journal</h3>
          <p className="text-gray-600">
            Document your experiences using the proven STAR method (Situation, Task, Action, Result) framework.
          </p>
        </div>
        <div className="bg-white rounded-lg shadow-md p-6">
          <h3 className="text-xl font-bold mb-3">📊 Progress Tracking</h3>
          <p className="text-gray-600">
            Receive detailed feedback and track your improvement across multiple interview sessions.
          </p>
        </div>
      </div>

      {/* How It Works */}
      <div className="bg-white rounded-lg shadow-md p-6">
        <h2 className="text-2xl font-bold mb-4">How MockMate Works</h2>
        <div className="grid md:grid-cols-4 gap-4 text-center">
          <div>
            <div className="bg-blue-100 rounded-full w-12 h-12 flex items-center justify-center mx-auto mb-2">
              <span className="text-blue-600 font-bold">1</span>
            </div>
            <h4 className="font-semibold">Create Profile</h4>
            <p className="text-sm text-gray-600">Sign up and set up your university and study field</p>
          </div>
          <div>
            <div className="bg-blue-100 rounded-full w-12 h-12 flex items-center justify-center mx-auto mb-2">
              <span className="text-blue-600 font-bold">2</span>
            </div>
            <h4 className="font-semibold">Build Journal</h4>
            <p className="text-sm text-gray-600">Document experiences using STAR method</p>
          </div>
          <div>
            <div className="bg-blue-100 rounded-full w-12 h-12 flex items-center justify-center mx-auto mb-2">
              <span className="text-blue-600 font-bold">3</span>
            </div>
            <h4 className="font-semibold">Practice Sessions</h4>
            <p className="text-sm text-gray-600">Join video sessions with other students</p>
          </div>
          <div>
            <div className="bg-blue-100 rounded-full w-12 h-12 flex items-center justify-center mx-auto mb-2">
              <span className="text-blue-600 font-bold">4</span>
            </div>
            <h4 className="font-semibold">Get Feedback</h4>
            <p className="text-sm text-gray-600">Receive structured feedback and improve</p>
          </div>
        </div>
      </div>
    </div>
  );
}
