import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { journalApi } from '../../services/journalApi';
import type { JournalFormData } from '../../types/journal';
import LoadingSpinner from '../common/LoadingSpinner';

const JournalForm: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEditing = Boolean(id);

  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<JournalFormData>({
    question: '',
    situation: '',
    task: '',
    action: '',
    result: '',
    category: ''
  });

  const categories = [
    'Software Development',
    'Leadership',
    'Communication',
    'Problem Solving',
    'Teamwork',
    'Database Development',
    'Project Management',
    'Customer Service',
    'Innovation',
    'Conflict Resolution'
  ];

  useEffect(() => {
    if (isEditing && id) {
      loadEntry(parseInt(id));
    }
  }, [isEditing, id]);

  const loadEntry = async (entryId: number) => {
    try {
      setLoading(true);
      const entry = await journalApi.getById(entryId);
      setFormData({
        question: entry.question,
        situation: entry.situation,
        task: entry.task,
        action: entry.action,
        result: entry.result,
        category: entry.category
      });
    } catch (err) {
      setError('Failed to load journal entry. Please try again.');
      console.error('Error loading journal entry:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (field: keyof JournalFormData, value: string | boolean) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.question.trim() || 
        !formData.situation.trim() || !formData.task.trim() || 
        !formData.action.trim() || !formData.result.trim()) {
      setError('Please fill in all required fields.');
      return;
    }

    try {
      setSaving(true);
      setError(null);

      const entryData = {
        question: formData.question.trim(),
        situation: formData.situation.trim(),
        task: formData.task.trim(),
        action: formData.action.trim(),
        result: formData.result.trim(),
        category: formData.category
      };

      if (isEditing && id) {
        await journalApi.update(parseInt(id), entryData);
      } else {
        await journalApi.create(entryData);
      }

      navigate('/journal');
    } catch (err) {
      setError(`Failed to ${isEditing ? 'update' : 'create'} journal entry. Please try again.`);
      console.error(`Error ${isEditing ? 'updating' : 'creating'} journal entry:`, err);
    } finally {
      setSaving(false);
    }
  };

  const handleCancel = () => {
    navigate('/journal');
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  return (
    <div className="container mx-auto px-6 py-8 max-w-4xl">
      {/* Header */}
      <div className="mb-8 text-center">
        <h1 className="text-4xl font-bold text-orange-800 mb-3">
          {isEditing ? 'Edit Journal Entry' : 'New Journal Entry'}
        </h1>
        <p className="text-orange-600 text-lg">
          Record your behavioral interview experience using the STAR method
        </p>
      </div>

      {/* Error Message */}
      {error && (
        <div className="bg-red-50 border-l-4 border-red-400 text-red-700 px-6 py-4 rounded-r-lg mb-6 shadow-sm">
          <div className="flex items-center">
            <svg className="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
              <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
            </svg>
            {error}
          </div>
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-8">
        {/* Question Section */}
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8 mb-8">
          <h2 className="text-2xl font-semibold text-orange-800 mb-6 flex items-center">
            <svg className="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            Interview Question
          </h2>
          
          <div>
            <label htmlFor="question" className="block text-sm font-semibold text-orange-700 mb-3">
              Behavioral Interview Question *
            </label>
            <textarea
              id="question"
              rows={3}
              value={formData.question}
              onChange={(e) => handleInputChange('question', e.target.value)}
              placeholder="e.g., Tell me about a time when you had to work with a difficult team member..."
              className="block w-full px-4 py-3 border-2 border-orange-200 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
              required
            />
          </div>
        </div>

        {/* STAR Method Section */}
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8 mb-8">
          <h2 className="text-2xl font-semibold text-orange-800 mb-6 flex items-center">
            <svg className="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z" />
            </svg>
            STAR Method Response
          </h2>
          
          {/* Situation */}
          <div className="mb-8">
            <label htmlFor="situation" className="block text-sm font-semibold text-orange-700 mb-3">
              <span className="bg-gradient-to-r from-orange-400 to-orange-500 text-white px-3 py-1 rounded-full text-sm font-bold mr-3 shadow-sm">
                S
              </span>
              Situation *
            </label>
            <p className="text-sm text-orange-600 mb-3 ml-12">
              Describe the context and background of the situation you were in.
            </p>
            <textarea
              id="situation"
              rows={4}
              value={formData.situation}
              onChange={(e) => handleInputChange('situation', e.target.value)}
              placeholder="Provide context about the situation, when and where it occurred, who was involved..."
              className="block w-full px-4 py-3 border-2 border-orange-200 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
              required
            />
          </div>

          {/* Task */}
          <div className="mb-8">
            <label htmlFor="task" className="block text-sm font-semibold text-orange-700 mb-3">
              <span className="bg-gradient-to-r from-green-400 to-green-500 text-white px-3 py-1 rounded-full text-sm font-bold mr-3 shadow-sm">
                T
              </span>
              Task *
            </label>
            <p className="text-sm text-orange-600 mb-3 ml-12">
              Explain what you needed to accomplish or what challenge you faced.
            </p>
            <textarea
              id="task"
              rows={4}
              value={formData.task}
              onChange={(e) => handleInputChange('task', e.target.value)}
              placeholder="Describe what you were responsible for or what needed to be achieved..."
              className="block w-full px-4 py-3 border-2 border-orange-200 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
              required
            />
          </div>

          {/* Action */}
          <div className="mb-8">
            <label htmlFor="action" className="block text-sm font-semibold text-orange-700 mb-3">
              <span className="bg-gradient-to-r from-yellow-400 to-yellow-500 text-white px-3 py-1 rounded-full text-sm font-bold mr-3 shadow-sm">
                A
              </span>
              Action *
            </label>
            <p className="text-sm text-orange-600 mb-3 ml-12">
              Detail the specific steps you took to address the task or challenge.
            </p>
            <textarea
              id="action"
              rows={5}
              value={formData.action}
              onChange={(e) => handleInputChange('action', e.target.value)}
              placeholder="Explain the specific actions you took, decisions you made, and how you implemented your approach..."
              className="block w-full px-4 py-3 border-2 border-orange-200 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
              required
            />
          </div>

          {/* Result */}
          <div>
            <label htmlFor="result" className="block text-sm font-semibold text-orange-700 mb-3">
              <span className="bg-gradient-to-r from-purple-400 to-purple-500 text-white px-3 py-1 rounded-full text-sm font-bold mr-3 shadow-sm">
                R
              </span>
              Result *
            </label>
            <p className="text-sm text-orange-600 mb-3 ml-12">
              Share the outcomes of your actions and what you learned from the experience.
            </p>
            <textarea
              id="result"
              rows={4}
              value={formData.result}
              onChange={(e) => handleInputChange('result', e.target.value)}
              placeholder="Describe the outcomes, impact of your actions, metrics if available, and lessons learned..."
              className="block w-full px-4 py-3 border-2 border-orange-200 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
              required
            />
          </div>
        </div>

        {/* Additional Details Section */}
        <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-8 mb-8">
          <h2 className="text-2xl font-semibold text-orange-800 mb-6 flex items-center">
            <svg className="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
            </svg>
            Category
          </h2>
          
          <div>
            <label htmlFor="category" className="block text-sm font-semibold text-orange-700 mb-3">
              Category *
            </label>
            <select
              id="category"
              value={formData.category}
              onChange={(e) => handleInputChange('category', e.target.value)}
              className="block w-full px-4 py-3 border-2 border-orange-200 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200 bg-white"
              required
            >
              <option value="">Select a category</option>
              {categories.map((category) => (
                <option key={category} value={category}>
                  {category}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Form Actions */}
        <div className="flex justify-end gap-4">
          <button
            type="button"
            onClick={handleCancel}
            className="px-6 py-3 border-2 border-orange-200 rounded-lg text-orange-700 hover:bg-orange-50 focus:outline-none focus:ring-2 focus:ring-orange-400 transition-colors duration-200 font-semibold"
            disabled={saving}
          >
            Cancel
          </button>
          <button
            type="submit"
            disabled={saving}
            className={`
              px-8 py-3 rounded-lg font-semibold transition-all duration-200 flex items-center gap-2
              ${saving 
                ? 'bg-orange-300 text-orange-500 cursor-not-allowed' 
                : 'bg-gradient-to-r from-orange-400 to-orange-500 hover:from-orange-500 hover:to-orange-600 text-white shadow-lg hover:shadow-xl transform hover:-translate-y-0.5'
              }
            `}
          >
            {saving && (
              <svg className="animate-spin h-5 w-5 text-current" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
            )}
            {saving ? 'Saving...' : (isEditing ? 'Update Entry' : 'Create Entry')}
          </button>
        </div>
      </form>
    </div>
  );
};

export default JournalForm;
