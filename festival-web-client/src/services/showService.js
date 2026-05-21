/**
 * Base URL for the C# REST API.
 * Ensure the backend server is running on this port.
 */
const API_BASE_URL = 'http://localhost:5050/festival/shows';

/**
 * Service object containing all HTTP operations for the Show entity.
 * Uses the native Fetch API to communicate with the backend.
 */
export const showService = {
    /**
     * Retrieves all shows from the database.
     * @returns {Promise<Array>} A promise that resolves to an array of show objects.
     */
    getAllShows: async () => {
        const response = await fetch(API_BASE_URL);
        if (!response.ok) throw new Error('Failed to fetch shows');
        return await response.json();
    },

    /**
     * Retrieves shows filtered by a specific date.
     * @param {string} date - The target date (format: YYYY-MM-DD).
     * @returns {Promise<Array>} A promise that resolves to an array of filtered shows.
     */
    filterShowsByDate: async (date) => {
        const response = await fetch(`${API_BASE_URL}/filter?date=${date}`);
        if (!response.ok) throw new Error('Failed to filter shows');
        return await response.json();
    },

    /**
     * Creates a new show record in the database.
     * @param {Object} show - The show data to be created (ID should be 0 or omitted).
     * @returns {Promise<Object>} A promise that resolves to the newly created show.
     */
    createShow: async (show) => {
        const response = await fetch(API_BASE_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(show),
        });
        if (!response.ok) throw new Error('Failed to create show');
        return await response.json();
    },

    /**
     * Updates an existing show record.
     * @param {number} id - The unique identifier of the show to update.
     * @param {Object} show - The updated show data.
     * @returns {Promise<void>}
     */
    updateShow: async (id, show) => {
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(show),
        });
        if (!response.ok) throw new Error('Failed to update show');
    },

    /**
     * Deletes a show record from the database.
     * @param {number} id - The unique identifier of the show to delete.
     * @returns {Promise<void>}
     */
    deleteShow: async (id) => {
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'DELETE',
        });
        if (!response.ok) throw new Error('Failed to delete show');
    }
};