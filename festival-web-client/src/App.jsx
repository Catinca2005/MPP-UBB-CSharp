import React, { useState, useEffect } from 'react';
import ShowTable from './components/ShowTable';
import ControlPanel from './components/ControlPanel';
import { showService } from './services/showService';

/**
 * Main application component.
 * Serves as the root container and state manager for the Festival Management UI.
 */
function App() {
    // Application State
    const [shows, setShows] = useState([]);
    const [selectedShowId, setSelectedShowId] = useState(null);

    // Lifecycle hook: Fetch data when the component mounts (loads)
    useEffect(() => {
        loadAllShows();
    }, []);

    /**
     * Retrieves all shows from the backend and updates the UI.
     */
    const loadAllShows = async () => {
        try {
            const data = await showService.getAllShows();
            setShows(data);
        } catch (error) {
            console.error("Failed to load shows:", error);
            alert("Error loading shows. Make sure the C# server is running.");
        }
    };

    /**
     * Filters the grid based on the selected date.
     * @param {string} date - The date to filter by.
     */
    const handleFilter = async (date) => {
        if (!date) {
            loadAllShows(); // Reset if date is cleared
            return;
        }
        
        try {
            const filteredData = await showService.filterShowsByDate(date);
            setShows(filteredData);
            setSelectedShowId(null); // Clear selection on filter
        } catch (error) {
            console.error("Filter failed:", error);
        }
    };

    /**
     * Processes a ticket purchase for the currently selected show.
     * Note: In a real REST API, this might be a custom endpoint (e.g., POST /buy).
     * Here we simulate it by updating the available seats via PUT.
     */
    const handleBuyTicket = async (buyerName, quantity) => {
        const targetShow = shows.find(s => s.id === selectedShowId);
        
        if (targetShow.availableSeats < quantity) {
            alert("Not enough available seats!");
            return;
        }

        // Prepare the updated object
        const updatedShow = {
            ...targetShow,
            availableSeats: targetShow.availableSeats - quantity,
            soldSeats: targetShow.soldSeats + quantity
        };

        try {
            await showService.updateShow(targetShow.id, updatedShow);
            alert(`Purchase successful for ${buyerName}!`);
            loadAllShows(); // Refresh the table to see the new seat count
        } catch (error) {
            console.error("Purchase failed:", error);
            alert("Failed to complete purchase.");
        }
    };

    return (
        <div style={{ padding: '20px', fontFamily: 'Segoe UI, Tahoma, Geneva, Verdana, sans-serif' }}>
            <h2>Festival Management System</h2>
            
            <ShowTable 
                shows={shows} 
                selectedShowId={selectedShowId} 
                onSelectShow={setSelectedShowId} 
            />
            
            <ControlPanel 
                onFilter={handleFilter} 
                onBuyTicket={handleBuyTicket} 
                hasSelectedShow={selectedShowId !== null} 
            />
        </div>
    );
}

export default App;