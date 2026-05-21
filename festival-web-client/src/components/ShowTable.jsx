import React from 'react';
import './ShowTable.css'; // Optional: For simple table borders

/**
 * Component responsible for rendering the grid of shows.
 * Applies visual cues (red background) for sold-out events.
 */
function ShowTable({ shows, selectedShowId, onSelectShow }) {
    return (
        <div className="table-container">
            <table border="1" cellPadding="8" style={{ width: '100%', textAlign: 'left', borderCollapse: 'collapse' }}>
                <thead>
                    <tr style={{ backgroundColor: '#f2f2f2' }}>
                        <th>Id</th>
                        <th>ArtistId</th>
                        <th>Date</th>
                        <th>Time</th>
                        <th>Location</th>
                        <th>AvailableSeats</th>
                    </tr>
                </thead>
                <tbody>
                    {shows.map((show) => {
                        // Determine row styles based on availability and selection
                        const isSoldOut = show.availableSeats === 0;
                        const isSelected = show.id === selectedShowId;
                        
                        let rowStyle = {};
                        if (isSoldOut) {
                            rowStyle = { backgroundColor: 'red', color: 'white' };
                        } else if (isSelected) {
                            rowStyle = { backgroundColor: '#0078D7', color: 'white' }; // Windows selection blue
                        }

                        return (
                            <tr 
                                key={show.id} 
                                style={rowStyle} 
                                onClick={() => onSelectShow(show.id)}
                                className="clickable-row"
                            >
                                <td>{show.id}</td>
                                <td>{show.artistId}</td>
                                {/* Format date to a readable string */}
                                <td>{new Date(show.date).toLocaleDateString()}</td>
                                <td>{show.time}</td>
                                <td>{show.location}</td>
                                <td>{show.availableSeats}</td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );
}

export default ShowTable;