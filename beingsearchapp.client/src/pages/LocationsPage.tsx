import React, { useState } from 'react';
import LocationsList from '../components/LocationsList';

const days = [
    'Sunday',
    'Monday',
    'Tuesday',
    'Wednesday',
    'Thursday',
    'Friday',
    'Saturday'
];

const LocationsPage: React.FC = () => {
    const [selectedDay, setSelectedDay] = useState<string | undefined>(undefined);

    const handleDayChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setSelectedDay(value === '' ? undefined : value);
    };

    return (
        <div className="container my-5">
            <h1 className="mb-4">Locations Available Between 10 AM and 1 PM</h1>

            <div className="row mb-4">
                <div className="col-md-6">
                    <div className="form-group">
                        <label htmlFor="daySelect" className="form-label">Select Day:</label>
                        <select
                            id="daySelect"
                            className="form-select"
                            value={selectedDay || ''}
                            onChange={handleDayChange}
                        >
                            <option value="">Today</option>
                            {days.map((day, index) => (
                                <option key={index} value={day}>{day}</option>
                            ))}
                        </select>
                    </div>
                </div>
            </div>

            <LocationsList day={selectedDay} />
        </div>
    );
};

export default LocationsPage;