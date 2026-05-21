import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import ShowTable from './components/ShowTable';
import ControlPanel from './components/ControlPanel';
import { showService } from './services/showService';

/**
 * Main application component.
 * Serves as the root container and state manager for the Festival Management UI.
 * Implements SignalR for real-time (Observer) updates across all connected clients.
 */
function App() {
    // Application State
    const [shows, setShows] = useState([]);
    const [selectedShowId, setSelectedShowId] = useState(null);

    // Lifecycle hook: Initialize data and WebSocket connection on mount
    useEffect(() => {
        loadAllShows();
        
        // --- SIGNALR WEBSOCKET SETUP ---
        
        // 1. Build the connection to the C# Hub
        const connection = new HubConnectionBuilder()
            .withUrl("http://localhost:5050/showHub") // Must match the endpoint in C# Program.cs
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect() // Auto-reconnect if the server drops
            .build();

        // 2. Define what happens when the server broadcasts a message
        // "ReceiveShowUpdate" matches the exact string used in ShowController.cs
        connection.on("ReceiveShowUpdate", (actionType) => {
            console.log(`[WebSocket] Server notification received: Item ${actionType}. Refreshing table...`);
            
            // Automatically fetch the fresh data from the database
            loadAllShows();
        });

        // 3. Start the connection
        const startConnection = async () => {
            try {
                await connection.start();
                console.log("[WebSocket] Successfully connected to SignalR Hub.");
            } catch (error) {
                console.error("[WebSocket] Connection failed: ", error);
            }
        };

        startConnection();

        // 4. Cleanup function: Close connection when the component unmounts (best practice)
        return () => {
            connection.stop();
        };
    }, []); // Empty dependency array ensures this runs only once

    /**
     * Retrieves all shows from the backend and updates the UI.
     */
    const loadAllShows = async () => {
        try {
            const data = await showService.getAllShows();
            setShows(data);
        } catch (error) {
            console.error("Failed to load shows:", error);
            // alert("Error loading shows. Make sure the C# server is running."); 
            // Commented out alert to prevent spamming if the server restarts
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
     */
    const handleBuyTicket = async (buyerName, quantity) => {
        const targetShow = shows.find(s => s.id === selectedShowId);
        
        if (targetShow.availableSeats < quantity) {
            alert("Not enough available seats!");
            return;
        }

        const updatedShow = {
            ...targetShow,
            availableSeats: targetShow.availableSeats - quantity,
            soldSeats: targetShow.soldSeats + quantity
        };

        try {
            // Note: We don't call loadAllShows() here manually anymore!
            // The PUT request updates the DB, the C# Controller shouts to the Hub, 
            // and the Hub tells ALL clients (including this one) to trigger loadAllShows().
            await showService.updateShow(targetShow.id, updatedShow);
            alert(`Purchase successful for ${buyerName}!`);
            setSelectedShowId(null); // Clear selection after buy
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