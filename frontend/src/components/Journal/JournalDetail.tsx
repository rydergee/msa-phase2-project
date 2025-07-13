import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { journalApi } from '../../services/journalApi';
import type { JournalEntry } from '../../types/journal';
import LoadingSpinner from '../common/LoadingSpinner';

const JournalDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  
  const [entry, setEntry] = useState<JournalEntry | null>(null);
  const [loading, setLoading] = useState(true);
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      loadEntry(parseInt(id));
    }
  }, [id]);

  const loadEntry = async (entryId: number) => {
    try {
      setLoading(true);
      setError(null);
      const data = await journalApi.getById(entryId);
      setEntry(data);
    } catch (err) {
      setError('Failed to load journal entry. It may have been deleted.');
      console.error('Error loading journal entry:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!entry || !window.confirm('Are you sure you want to delete this journal entry? This action cannot be undone.')) {
      return;
    }

    try {
      setDeleting(true);
      await journalApi.delete(entry.id);
      navigate('/journal');
    } catch (err) {
      setError('Failed to delete journal entry. Please try again.');
      console.error('Error deleting journal entry:', err);
    } finally {
      setDeleting(false);
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  if (error && !entry) {
    return (
      <div className="container mx-auto px-4 py-8 max-w-4xl">
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
          {error}
        </div>
        <div className="mt-4">
          <Link
            to="/journal"
            className="text-blue-600 hover:text-blue-800 font-medium"
          >
            ← Back to Journal
          </Link>
        </div>
      </div>
    );
  }

  if (!entry) {
    return (
      <div className="container mx-auto px-4 py-8 max-w-4xl">
        <div className="text-center">
          <p className="text-gray-500">Journal entry not found.</p>
          <Link
            to="/journal"
            className="text-blue-600 hover:text-blue-800 font-medium mt-4 inline-block"
          >
            ← Back to Journal
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8 max-w-4xl">
      {/* Header */}
      <div className="mb-8">
        <div className="flex items-center justify-between">
          <Link
            to="/journal"
            className="text-blue-600 hover:text-blue-800 font-medium flex items-center gap-2"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
            </svg>
            Back to Journal
          </Link>
          
          <div className="flex items-center gap-2">
            <Link
              to={`/journal/edit/${entry.id}`}
              className="px-4 py-2 text-blue-600 border border-blue-600 rounded-md hover:bg-blue-50 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              Edit
            </Link>
            <button
              onClick={handleDelete}
              disabled={deleting}
              className="px-4 py-2 text-red-600 border border-red-600 rounded-md hover:bg-red-50 focus:outline-none focus:ring-2 focus:ring-red-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {deleting ? 'Deleting...' : 'Delete'}
            </button>
          </div>
        </div>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
          {error}
        </div>
      )}

      {/* Entry Header */}
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 mb-6">
        <div className="flex items-start justify-between mb-4">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 mb-2">
              Journal Entry
            </h1>
            <div className="flex items-center gap-4 text-sm text-gray-600">
              <span>Created: {formatDate(entry.createdAt)}</span>
              {entry.updatedAt !== entry.createdAt && (
                <span>Updated: {formatDate(entry.updatedAt)}</span>
              )}
              <span className="bg-gray-100 text-gray-800 px-2 py-1 rounded text-xs font-medium">
                Private
              </span>
            </div>
          </div>
          
          <div className="text-right">
            {entry.category && (
              <div className="bg-blue-100 text-blue-800 px-3 py-1 rounded-full text-sm font-medium mb-2">
                {entry.category}
              </div>
            )}
            {entry.timesReviewed > 0 && (
              <div className="text-sm text-gray-600">
                Reviewed {entry.timesReviewed} time{entry.timesReviewed !== 1 ? 's' : ''}
              </div>
            )}
          </div>
        </div>

        {/* Question */}
        <div className="bg-gray-50 border border-gray-200 rounded-lg p-4">
          <h2 className="text-lg font-semibold text-gray-900 mb-2">Interview Question</h2>
          <p className="text-gray-700 italic">"{entry.question}"</p>
        </div>
      </div>

      {/* STAR Method Response */}
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 mb-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-6">STAR Method Response</h2>
        
        {/* Situation */}
        <div className="mb-8">
          <div className="flex items-center mb-3">
            <span className="bg-blue-100 text-blue-800 px-3 py-1 rounded-full text-sm font-semibold mr-3">
              S
            </span>
            <h3 className="text-lg font-semibold text-gray-900">Situation</h3>
          </div>
          <div className="ml-12">
            <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">{entry.situation}</p>
          </div>
        </div>

        {/* Task */}
        <div className="mb-8">
          <div className="flex items-center mb-3">
            <span className="bg-green-100 text-green-800 px-3 py-1 rounded-full text-sm font-semibold mr-3">
              T
            </span>
            <h3 className="text-lg font-semibold text-gray-900">Task</h3>
          </div>
          <div className="ml-12">
            <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">{entry.task}</p>
          </div>
        </div>

        {/* Action */}
        <div className="mb-8">
          <div className="flex items-center mb-3">
            <span className="bg-yellow-100 text-yellow-800 px-3 py-1 rounded-full text-sm font-semibold mr-3">
              A
            </span>
            <h3 className="text-lg font-semibold text-gray-900">Action</h3>
          </div>
          <div className="ml-12">
            <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">{entry.action}</p>
          </div>
        </div>

        {/* Result */}
        <div>
          <div className="flex items-center mb-3">
            <span className="bg-purple-100 text-purple-800 px-3 py-1 rounded-full text-sm font-semibold mr-3">
              R
            </span>
            <h3 className="text-lg font-semibold text-gray-900">Result</h3>
          </div>
          <div className="ml-12">
            <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">{entry.result}</p>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="mt-8 flex justify-center gap-4">
        <Link
          to={`/journal/edit/${entry.id}`}
          className="px-6 py-3 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 font-medium"
        >
          Edit Entry
        </Link>
        <button
          onClick={() => window.print()}
          className="px-6 py-3 border border-gray-300 text-gray-700 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 font-medium"
        >
          Print
        </button>
      </div>
    </div>
  );
};

export default JournalDetail;
