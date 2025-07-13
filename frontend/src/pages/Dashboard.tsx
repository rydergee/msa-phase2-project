export default function Dashboard() {
  return (
    <div className="max-w-6xl mx-auto">
      <div className="mb-8">
        <h1 className="text-4xl font-bold text-orange-800 mb-3">Dashboard</h1>
        <p className="text-orange-600">Welcome back! Here's an overview of your interview preparation journey.</p>
      </div>
      
      <div className="grid md:grid-cols-3 gap-6 mb-8">
        {/* Quick Stats */}
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-6">
          <div className="flex items-center">
            <div className="bg-gradient-to-r from-orange-400 to-orange-500 rounded-full p-3 mr-4">
              <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
            </div>
            <div>
              <p className="text-2xl font-bold text-orange-800">0</p>
              <p className="text-orange-600">Journal Entries</p>
            </div>
          </div>
        </div>
        
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-6">
          <div className="flex items-center">
            <div className="bg-gradient-to-r from-orange-400 to-orange-500 rounded-full p-3 mr-4">
              <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 10l4.553-2.276A1 1 0 0121 8.618v6.764a1 1 0 01-1.447.894L15 14M5 18h8a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z" />
              </svg>
            </div>
            <div>
              <p className="text-2xl font-bold text-orange-800">0</p>
              <p className="text-orange-600">Practice Sessions</p>
            </div>
          </div>
        </div>
        
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-6">
          <div className="flex items-center">
            <div className="bg-gradient-to-r from-orange-400 to-orange-500 rounded-full p-3 mr-4">
              <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
              </svg>
            </div>
            <div>
              <p className="text-2xl font-bold text-orange-800">-</p>
              <p className="text-orange-600">Progress Score</p>
            </div>
          </div>
        </div>
      </div>
      
      <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8">
        <h2 className="text-2xl font-bold text-orange-800 mb-4">Getting Started</h2>
        <p className="text-orange-600 mb-6">
          Your dashboard will show session management, journal entries, and analytics as you use MockMate.
        </p>
        <div className="space-y-3">
          <div className="flex items-center">
            <div className="w-2 h-2 bg-orange-400 rounded-full mr-3"></div>
            <span className="text-orange-700">Start by creating your first journal entry</span>
          </div>
          <div className="flex items-center">
            <div className="w-2 h-2 bg-orange-400 rounded-full mr-3"></div>
            <span className="text-orange-700">Practice common behavioral interview questions</span>
          </div>
          <div className="flex items-center">
            <div className="w-2 h-2 bg-orange-400 rounded-full mr-3"></div>
            <span className="text-orange-700">Schedule practice sessions with peers</span>
          </div>
        </div>
      </div>
    </div>
  );
}
