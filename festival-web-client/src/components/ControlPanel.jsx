import React, { useState } from 'react';

/**
 * Component handling user inputs for filtering and purchasing tickets.
 */
function ControlPanel({ onFilter, onBuyTicket, hasSelectedShow }) {
    // Local state for the input fields
    const [filterDate, setFilterDate] = useState('');
    const [buyerName, setBuyerName] = useState('');
    const [quantity, setQuantity] = useState('');

    const handleFilterClick = () => {
        onFilter(filterDate);
    };

    const handleBuyClick = () => {
        if (!hasSelectedShow) {
            alert("Please select a show from the table first.");
            return;
        }
        if (!buyerName || !quantity) {
            alert("Please provide both buyer name and quantity.");
            return;
        }
        
        onBuyTicket(buyerName, parseInt(quantity, 10));
        
        // Clear inputs after purchase attempt
        setBuyerName('');
        setQuantity('');
    };

    return (
        <div style={{ marginTop: '20px' }}>
            <hr />
            {/* Filter Section */}
            <div style={{ marginBottom: '20px', display: 'flex', gap: '10px' }}>
                <input 
                    type="date" 
                    value={filterDate} 
                    onChange={(e) => setFilterDate(e.target.value)} 
                />
                <button onClick={handleFilterClick}>Filter by</button>
            </div>

            {/* Purchase Section */}
            <div style={{ display: 'flex', flexDirection: 'column', gap: '10px', width: '300px' }}>
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <label>Buyer Name:</label>
                    <input 
                        type="text" 
                        value={buyerName} 
                        onChange={(e) => setBuyerName(e.target.value)} 
                    />
                </div>
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <label>Quantity:</label>
                    <input 
                        type="number" 
                        min="1" 
                        value={quantity} 
                        onChange={(e) => setQuantity(e.target.value)} 
                        style={{ width: '60px' }}
                    />
                    <button onClick={handleBuyClick}>Buy Tickets</button>
                </div>
            </div>
        </div>
    );
}

export default ControlPanel;