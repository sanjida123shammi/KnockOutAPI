// Image Binding
ko.bindingHandlers.fileUpload = {
    init: function (element, valueAccessor, allBindings) {
        $(element).change(function () {
            var file = this.files[0];
            var targetObservable = valueAccessor();

            if (file) {
                targetObservable(file);
                var previewObservable = allBindings.get('imagePreview');
                if (previewObservable) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        previewObservable(e.target.result);
                    };
                    reader.readAsDataURL(file);
                }
            }
            else {
                targetObservable(null);
                var previewObservable = allBindings.get('imagePreview');
                if (previewObservable) {
                    previewObservable(null);
                }
            }
        });
    }
}

// Model for individual GuestFacility
function GuestFacilityViewModel(data, facilityList) {
    var self = this;
    data = data || {};

    self.GuestFacilityId = ko.observable(data.GuestFacilityId || 0);
    self.GuestId = ko.observable(data.GuestId || 0);
    self.FacilityId = ko.observable(data.FacilityId || 0);
    self.TotalVisits = ko.observable(data.TotalVisits || 1);
    self.Price = ko.observable(data.Price || 0.00);

    self.Subtotal = ko.computed(function () {
        return (parseFloat(self.TotalVisits()) || 0) * (parseFloat(self.Price()) || 0);
    });

    self.FacilityId.subscribe(function (newFacilityId) {
        if (facilityList && newFacilityId) {
            var selectedFacility = ko.utils.arrayFirst(facilityList(), function (f) {
                return f.FacilityId == newFacilityId;
            });
            if (selectedFacility && selectedFacility.Price) {
                self.Price(selectedFacility.Price);
            }
        }
    });
}

// Main View Model
function GuestViewModel() {
    var self = this;
    var apiBaseUrl = '/api/Guests';
    var apiFacilitiesUrl = '/api/Guests/GetFacilities';

    self.guests = ko.observableArray([]);
    self.facilities = ko.observableArray([]);

    self.guestId = ko.observable(0);
    self.guestName = ko.observable('');
    self.contact = ko.observable('');
    self.checkOut = ko.observable(new Date().toISOString().split('T')[0]);
    self.isPaid = ko.observable(false);
    self.imageUrl = ko.observable('');
    self.imagePreviewUrl = ko.observable('');

    self.selectedFile = ko.observable(null);
    self.guestFacilities = ko.observableArray([]);
    self.isEditing = ko.observable(false);

    self.loadFacilities = function () {
        $.ajax({
            url: apiFacilitiesUrl,
            type: 'GET',
            success: function (data) {
                self.facilities(data);
            },
            error: function (err) {
                alert('Error fetching products list: ' + err.statusText);
            }
        });
    };

    self.addGuestFacility = function () {
        self.guestFacilities.push(new GuestFacilityViewModel(null, self.facilities));
    };

    self.removeGuestFacility = function (item) {
        self.guestFacilities.remove(item);
    };

    self.loadGuests = function () {
        $.ajax({
            url: apiBaseUrl,
            type: 'GET',
            success: function (data) {
                self.guests(data);
            },
            error: function (err) {
                alert('Error fetching orders: ' + err.statusText)
            }
        });
    };

    self.saveGuest = function () {
        if (self.guestFacilities().length === 0) {
            alert('Add atleast one Guest Facility');
            return;
        }
        var guestDto = {
            GuestId: self.guestId(),
            GuestName: self.guestName(),
            Contact: self.contact(),
            CheckOut: self.checkOut(),
            Ispaid: self.isPaid(),
            ImageUrl: self.imageUrl(),
            GuestFacilities: ko.toJS(self.guestFacilities)
        };
        var formData = new FormData();
        formData.append("Guest", JSON.stringify(guestDto));

        if (self.selectedFile()) {
            formData.append("ImageFile", self.selectedFile());
        }
        var isUpdate = self.isEditing();
        var requestUrl = isUpdate ? apiBaseUrl + '/' + self.guestId() : apiBaseUrl;
        var requestType = isUpdate ? 'PUT' : 'POST';

        $.ajax({
            url: requestUrl,
            type: requestType,
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                alert(response);
                self.resetForm();
                self.loadGuests();
            },
            error: function (xhr) {
                alert('Operation failed: ' + xhr.responseText);
            }
        });
    };

    self.editGuest = function (guest) {
        self.isEditing(true);
        self.guestId(guest.GuestId);
        self.guestName(guest.GuestName);
        self.contact(guest.Contact);

        var formattedDate = guest.CheckOut ? guest.CheckOut.substring(0, 10) : '';
        self.checkOut(formattedDate);
        self.isPaid(guest.IsPaid);
        self.imageUrl(guest.ImageUrl);
        self.imagePreviewUrl('');

        self.guestFacilities([]);
        if (guest.GuestFacilities && guest.GuestFacilities.length > 0) {
            var mapped = $.map(guest.GuestFacilities, function (item) {
                return new GuestFacilityViewModel(item, self.facilities);
            });
            self.guestFacilities(mapped);
        }
    };

    self.deleteGuest = function (guest) {
        if (confirm('Are you sure you want to delete Guest #' + guest.GuestName + '?')) {
            $.ajax({
                url: apiBaseUrl + '/' + guest.GuestId,
                type: 'DELETE',
                success: function (response) {
                    alert(response);
                    self.loadGuests();
                },
                error: function (error) {
                    alert('Failed to delete this guest');
                }
            });
        }
    };

    self.resetForm = function () {
        self.isEditing(false);
        self.guestId(0);
        self.guestName('');
        self.contact('');
        self.checkOut(new Date().toISOString().split('T')[0]);
        self.isPaid(false);
        self.imageUrl('');
        self.imagePreviewUrl('');
        self.selectedFile(null);
        self.guestFacilities([]);
        $('#imageFileInput').val('')
    };
    self.loadFacilities();
    self.loadGuests();
}

$(document).ready(function () {
    ko.applyBindings(new GuestViewModel());
});