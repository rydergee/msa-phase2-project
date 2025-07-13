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
    <div className="container mx-auto px-4 py-8 max-w-4xl">
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">
          {isEditing ? 'Edit Journal Entry' : 'New Journal Entry'}
        </h1>
        <p className="text-gray-600 mt-2">
          Record your behavioral interview experience using the STAR method
        </p>
      </div>

      {/* Error Message */}
      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-8">
        {/* Question Section */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Interview Question</h2>
          
          <div>
            <label htmlFor="question" className="block text-sm font-medium text-gray-700 mb-2">
              Behavioral Interview Question *
            </label>
            <textarea
              id="question"
              rows={3}
              value={formData.question}
              onChange={(e) => handleInputChange('question', e.target.value)}
              placeholder="e.g., Tell me about a time when you had to work with a difficult team member..."
              className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              required
            />
          </div>
        </div>

        {/* STAR Method Section */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">STAR Method Response</h2>
          
          {/* Situation */}
          <div className="mb-6">
            <label htmlFor="situation" className="block text-sm font-medium text-gray-700 mb-2">
              <span className="bg-blue-100 text-blue-800 px-2 py-1 rounded text-xs font-semibold mr-2">S</span>
              Situation *
            </label>
            <p className="text-sm text-gray-600 mb-2">
              Describe the context and background of the situation you were in.
            </p>
            <textarea
              id="situation"
              rows={4}
              value={formData.situation}
              onChange={(e) => handleInputChange('situation', e.target.value)}
              placeholder="Provide context about the situation, when and where it occurred, who was involved..."
              className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              required
            />
          </div>

          {/* Task */}
          <div className="mb-6">
            <label htmlFor="task" className="block text-sm font-medium text-gray-700 mb-2">
              <span className="bg-green-100 text-green-800 px-2 py-1 rounded text-xs font-semibold mr-2">T</span>
              Task *
            </label>
            <p className="text-sm text-gray-600 mb-2">
              Explain what you needed to accomplish or what challenge you faced.
            </p>
            <textarea
              id="task"
              rows={4}
              value={formData.task}
              onChange={(e) => handleInputChange('task', e.target.value)}
              placeholder="Describe what you were responsible for or what needed to be achieved..."
              className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              required
            />
          </div>

          {/* Action */}
          <div className="mb-6">
            <label htmlFor="action" className="block text-sm font-medium text-gray-700 mb-2">
              <span className="bg-yellow-100 text-yellow-800 px-2 py-1 rounded text-xs font-semibold mr-2">A</span>
              Action *
            </label>
            <p className="text-sm text-gray-600 mb-2">
              Detail the specific steps you took to address the task or challenge.
            </p>
            <textarea
              id="action"
              rows={5}
              value={formData.action}
              onChange={(e) => handleInputChange('action', e.target.value)}
              placeholder="Explain the specific actions you took, decisions you made, and how you implemented your approach..."
              className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              required
            />
          </div>

          {/* Result */}
          <div>
            <label htmlFor="result" className="block text-sm font-medium text-gray-700 mb-2">
              <span className="bg-purple-100 text-purple-800 px-2 py-1 rounded text-xs font-semibold mr-2">R</span>
              Result *
            </label>
            <p className="text-sm text-gray-600 mb-2">
              Share the outcomes of your actions and what you learned from the experience.
            </p>
            <textarea
              id="result"
              rows={4}
              value={formData.result}
              onChange={(e) => handleInputChange('result', e.target.value)}
              placeholder="Describe the outcomes, impact of your actions, metrics if available, and lessons learned..."
              className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              required
            />
          </div>
        </div>

        {/* Additional Details Section */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Category</h2>
          
          <div>
            <label htmlFor="category" className="block text-sm font-medium text-gray-700 mb-2">
              Category *
            </label>
            <select
              id="category"
              value={formData.category}
              onChange={(e) => handleInputChange('category', e.target.value)}
              className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
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
            className="px-6 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500"
            disabled={saving}
          >
            Cancel
          </button>
          <button
            type="submit"
            disabled={saving}
            className="px-6 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
          >
            {saving && <LoadingSpinner size="sm" />}
            {saving ? 'Saving...' : (isEditing ? 'Update Entry' : 'Create Entry')}
          </button>
        </div>
      </form>
    </div>
  );
};

export default JournalForm;
