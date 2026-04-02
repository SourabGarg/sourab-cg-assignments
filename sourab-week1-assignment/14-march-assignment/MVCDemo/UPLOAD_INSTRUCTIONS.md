# Dog Image Upload Feature - Setup Instructions

## Overview
Dog image uploads are now fully implemented and optional. Users can upload images when creating or editing dogs.

## Where Images Are Stored

### Folder Structure
Images are stored in: **`wwwroot/uploads/dogs/`**

```
MVCDemo/
├── wwwroot/
│   ├── uploads/
│   │   └── dogs/          <-- Dog images stored here
│   ├── css/
│   ├── js/
│   └── lib/
└── ...
```

### Important: Create the uploads Folder
You need to create the `wwwroot/uploads/dogs/` folder structure manually if it doesn't exist:

1. **Navigate to your project root** in File Explorer or terminal
2. **Go to**: `MVCDemo/wwwroot/`
3. **Create folders**: 
   - Create folder named `uploads` (if it doesn't exist)
   - Inside `uploads`, create folder named `dogs`

Or use PowerShell:
```powershell
mkdir MVCDemo\wwwroot\uploads\dogs -Force
```

## Features Implemented

### 1. Dog Model Updated
- Added `ImagePath` property (string, stores relative path in database)
- Added `ImageFile` property (IFormFile, for form uploads)

### 2. File Upload Handling
- **Upload Controller**: `DogController.cs` - `SaveImageAsync()` method
- Generates unique filenames using GUID to avoid conflicts
- Files are saved to `/uploads/dogs/` folder
- Relative path stored in database (e.g., `/uploads/dogs/guid-filename.jpg`)

### 3. File Size & Type Restrictions
**Client-side validation:**
- Accepted formats: JPG, PNG, GIF, WebP
- HTML5 `accept="image/*"` attribute enforces this

**Server-side validation** (recommended to add):
You can add validation in the `SaveImageAsync()` method:
```csharp
// File size check (5MB max)
if (imageFile.Length > 5 * 1024 * 1024)
    return null; // Reject files larger than 5MB

// MIME type check
string[] allowedMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };
if (!allowedMimeTypes.Contains(imageFile.ContentType))
    return null; // Reject invalid MIME types
```

## Usage

### Create Dog with Image
1. Go to "Create New Dog"
2. Fill in ID, Name, Age
3. (Optional) Click "Choose File" to upload an image
4. Click "Create Dog"

### Edit Dog Image
1. Go to "Dogs" list
2. Click "Edit" on a dog
3. View current image (if uploaded)
4. Click "Choose File" to upload a new image
5. Click "Update"

### Delete Dog
- When deleting a dog, the associated image file is automatically deleted from the server

## Pages Updated

✅ **Create.cshtml** - Image upload input added
✅ **Edit.cshtml** - Image upload input + display current image
✅ **Details.cshtml** - Display uploaded image with fallback emoji
✅ **Index.cshtml** - Display images in dog cards with fallback emoji
✅ **DogController.cs** - Image upload handling + file deletion
✅ **Dog.cs Model** - ImagePath and ImageFile properties added

## Image Storage Security

**Current Implementation:**
- Images stored outside of source control (in `/wwwroot/`)
- Unique filenames prevent overwriting
- Original filenames not preserved (security benefit)

**Future Improvements (Optional):**
- Add file size validation on server-side
- Add MIME type validation
- Consider adding image resize/compression
- Add virus scanning for production environments

## Troubleshooting

### Images Not Uploading?
1. Check that `/wwwroot/uploads/dogs/` folder exists
2. Verify folder has write permissions
3. Check browser console for errors
4. Verify file is under 5MB and in supported format

### Images Not Displaying?
1. Check that the image file exists in `/wwwroot/uploads/dogs/`
2. Verify the ImagePath in database matches actual file location
3. Check browser's Network tab for 404 errors on image requests

### Delete Causes Error?
- The delete method gracefully handles missing image files (won't crash if file not found)
- Check server logs for any permission issues
