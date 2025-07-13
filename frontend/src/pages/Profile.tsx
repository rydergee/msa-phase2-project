export default function Profile() {
  return (
    <div className="max-w-4xl mx-auto">
      <div className="mb-8">
        <h1 className="text-4xl font-bold text-orange-800 mb-3">Profile</h1>
        <p className="text-orange-600">Manage your account information and preferences.</p>
      </div>
      
      <div className="grid md:grid-cols-2 gap-8">
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8">
          <h2 className="text-2xl font-bold text-orange-800 mb-6 flex items-center">
            <svg className="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
            </svg>
            Personal Information
          </h2>
          <p className="text-orange-600 mb-6">
            Profile editing and personal information management will be available soon.
          </p>
          <div className="space-y-3">
            <div className="flex items-center">
              <div className="w-2 h-2 bg-orange-400 rounded-full mr-3"></div>
              <span className="text-orange-700">Edit name and contact information</span>
            </div>
            <div className="flex items-center">
              <div className="w-2 h-2 bg-orange-400 rounded-full mr-3"></div>
              <span className="text-orange-700">Update university and study field</span>
            </div>
          </div>
        </div>
        
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8">
          <h2 className="text-2xl font-bold text-orange-800 mb-6 flex items-center">
            <svg className="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
            </svg>
            Security Settings
          </h2>
          <p className="text-orange-600 mb-6">
            Password management and security features will be implemented in upcoming updates.
          </p>
          <div className="space-y-3">
            <div className="flex items-center">
              <div className="w-2 h-2 bg-orange-400 rounded-full mr-3"></div>
              <span className="text-orange-700">Change password</span>
            </div>
            <div className="flex items-center">
              <div className="w-2 h-2 bg-orange-400 rounded-full mr-3"></div>
              <span className="text-orange-700">Two-factor authentication</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
