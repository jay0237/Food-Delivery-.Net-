document.addEventListener('DOMContentLoaded', () => {
    const locationButton = document.querySelector('.location-button');

    locationButton?.addEventListener('click', detectLocation);
    document.getElementById('useDetectedLocation')
        ?.addEventListener('click', useDetectedLocation);
});

function detectLocation() {
    const locationButton = document.querySelector('.location-button');

    if (!locationButton) {
        return;
    }

    if (!navigator.geolocation) {
        showNotification('Geolocation is not supported by your browser.', 'error');
        return;
    }

    const originalText = locationButton.textContent.trim();
    locationButton.disabled = true;
    locationButton.textContent = ' Detecting...';

    navigator.geolocation.getCurrentPosition(
        ({ coords }) => {
            try {
                sessionStorage.setItem('userLatitude', String(coords.latitude));
                sessionStorage.setItem('userLongitude', String(coords.longitude));
            } catch {
                locationButton.disabled = false;
                locationButton.textContent = originalText;
                showNotification('Location could not be saved in this browser.', 'error');
                return;
            }

            locationButton.disabled = false;
            locationButton.textContent = ' Location detected';
            showNotification(
                'Location detected. You can add its GPS coordinates to your delivery details.',
                'success'
            );
        },
        (error) => {
            locationButton.disabled = false;
            locationButton.textContent = originalText;

            const messages = {
                1: 'Location permission was denied. Enable location access in your browser settings.',
                2: 'Your location is currently unavailable.',
                3: 'The location request timed out.'
            };

            showNotification(messages[error.code] ?? 'Unable to detect your location.', 'error');
        },
        {
            enableHighAccuracy: true,
            timeout: 10000,
            maximumAge: 0
        }
    );
}

function useDetectedLocation() {
    let latitude;
    let longitude;

    try {
        latitude = sessionStorage.getItem('userLatitude');
        longitude = sessionStorage.getItem('userLongitude');
    } catch {
        showNotification('Location could not be read from this browser.', 'error');
        return;
    }

    if (!latitude || !longitude) {
        showNotification('Detect your location from the navigation bar first.', 'error');
        return;
    }

    document.getElementById('latitudeInput').value = latitude;
    document.getElementById('longitudeInput').value = longitude;
    showNotification('GPS coordinates added. Enter your street address above.', 'success');
}

function showNotification(message, type) {
    const notification = document.createElement('div');
    notification.className =
        `alert alert-${type === 'success' ? 'success' : 'danger'} position-fixed`;
    notification.setAttribute('role', 'status');
    notification.style.cssText = 'top: 80px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.append(document.createTextNode(message));

    document.body.appendChild(notification);

    setTimeout(() => notification.remove(), 5000);
}
