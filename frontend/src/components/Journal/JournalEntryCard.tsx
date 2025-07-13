import React from 'react';
import { Link } from 'react-router-dom';
import type { JournalEntry } from '../../types/journal';

interface JournalEntryCardProps {
  entry: JournalEntry;
  onDelete: (id: number) => void;
}

const JournalEntryCard: React.FC<JournalEntryCardProps> = ({ entry, onDelete }) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  const getCategoryColor = (category: string) => {
    const colors = {
      'Software Development': 'bg-orange-100 text-orange-800',
      'Leadership': 'bg-orange-100 text-orange-800',
      'Communication': 'bg-orange-100 text-orange-800',
      'Problem Solving': 'bg-orange-100 text-orange-800',
      'Teamwork': 'bg-orange-100 text-orange-800',
      'Database Development': 'bg-orange-100 text-orange-800',
      'Project Management': 'bg-orange-100 text-orange-800',
      'default': 'bg-orange-100 text-orange-800'
    };
    return colors[category as keyof typeof colors] || colors.default;
  };

  const truncateText = (text: string, maxLength: number = 100) => {
    if (text.length <= maxLength) return text;
    return text.substring(0, maxLength) + '...';
  };

  return (
    <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-6 hover:shadow-xl transition-all duration-200 transform hover:-translate-y-1">
      {/* Header */}
      <div className="flex justify-between items-start mb-4">
        <div className="flex-1">
          <div className="flex items-center gap-2 mb-3">
            <span className={`px-3 py-1 rounded-full text-xs font-semibold ${getCategoryColor(entry.category)}`}>
              {entry.category}
            </span>
          </div>
          <h3 className="text-lg font-semibold text-gray-900 mb-2">
            Journal Entry
          </h3>
        </div>
        
        {/* Actions Menu */}
        <div className="flex items-center gap-2">
          <Link
            to={`/journal/${entry.id}`}
            className="text-orange-600 hover:text-orange-800 p-2 rounded-lg hover:bg-orange-50 transition-colors"
            title="View entry"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
            </svg>
          </Link>
          <Link
            to={`/journal/${entry.id}/edit`}
            className="text-orange-600 hover:text-orange-800 p-2 rounded-lg hover:bg-orange-50 transition-colors"
            title="Edit entry"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
          </Link>
          <button
            onClick={() => onDelete(entry.id)}
            className="text-red-600 hover:text-red-800 p-2 rounded-lg hover:bg-red-50 transition-colors"
            title="Delete entry"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </div>
      </div>

      {/* Question */}
      <div className="mb-4">
        <h4 className="text-sm font-semibold text-orange-700 mb-2">Question:</h4>
        <p className="text-sm text-orange-600 italic leading-relaxed">
          {truncateText(entry.question, 120)}
        </p>
      </div>

      {/* STAR Preview */}
      <div className="space-y-3 mb-4">
        <div>
          <span className="text-xs font-semibold text-orange-500 uppercase tracking-wide">Situation:</span>
          <p className="text-sm text-orange-700 mt-1 leading-relaxed">
            {truncateText(entry.situation, 80)}
          </p>
        </div>
        <div>
          <span className="text-xs font-semibold text-orange-500 uppercase tracking-wide">Result:</span>
          <p className="text-sm text-orange-700 mt-1 leading-relaxed">
            {truncateText(entry.result, 80)}
          </p>
        </div>
      </div>

      {/* Footer */}
      <div className="flex justify-between items-center text-xs text-orange-500 pt-4 border-t border-orange-100">
        <div className="flex items-center gap-4">
          <span>Created {formatDate(entry.createdAt)}</span>
          <span>Reviewed {entry.timesReviewed} times</span>
        </div>
        <Link
          to={`/journal/${entry.id}`}
          className="text-orange-600 hover:text-orange-800 font-semibold transition-colors"
        >
          View Details →
        </Link>
      </div>
    </div>
  );
};

export default JournalEntryCard;
