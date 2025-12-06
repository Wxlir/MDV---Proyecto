# Unity Menu Setup Guide

## 🎮 Setting Up Your Horror Game Title Screen with Audio

I've created 3 scripts for you:

- `MenuAudioManager.cs` - Handles background music
- `ButtonSoundEffect.cs` - Plays sound when buttons are clicked
- `MenuController.cs` - Handles Play, Options, and Exit button functions

---

## 📋 Step-by-Step Setup Instructions

### **Part 1: Setting Up Background Music**

1. **In your Main Menu scene:**

   - Right-click in the Hierarchy window
   - Select `Create Empty`
   - Rename it to "AudioManager"

2. **Add the script:**

   - Select the AudioManager GameObject
   - In the Inspector, click `Add Component`
   - Search for "MenuAudioManager" and add it
   - It will automatically add an AudioSource component

3. **Assign the background music:**
   - In the Inspector, find the MenuAudioManager component
   - Look for the "Background Music" field
   - Click the small circle next to it (⊙)
   - In the popup, search for your background music file (probably "dark-horror-background" or "scary-horror-music")
   - Double-click to select it
   - Adjust the "Music Volume" slider if needed (default is 0.5)

---

### **Part 2: Adding Sound Effects to Buttons**

**Do this for EACH button (Play, Options, Exit):**

1. **Select the button GameObject** in the Hierarchy (the one with the Button component)

2. **Add the ButtonSoundEffect script:**

   - In the Inspector, click `Add Component`
   - Search for "ButtonSoundEffect" and add it
   - It will automatically add an AudioSource component

3. **Assign the click sound:**
   - Find the ButtonSoundEffect component in the Inspector
   - Look for the "Click Sound" field
   - Click the circle next to it (⊙)
   - Search for "menu option sfx"
   - Double-click to select it
   - Adjust "Sfx Volume" if needed (default is 1.0)

---

### **Part 3: Making Buttons Work (Play, Options, Exit)**

1. **Create a MenuController GameObject:**

   - Right-click in the Hierarchy
   - Select `Create Empty`
   - Rename it to "MenuController"

2. **Add the script:**

   - Select MenuController
   - In Inspector, click `Add Component`
   - Search for "MenuController" and add it

3. **Configure the game scene name:**

   - In the MenuController component
   - Find "Game Scene Name"
   - Type the exact name of your game scene (probably "SampleScene")
   - ⚠️ The name must match exactly what you see in your Scenes folder

4. **Connect the Play Button:**

   - Select your **Play button** in the Hierarchy
   - In the Inspector, find the **Button component**
   - Scroll down to "On Click ()"
   - Click the `+` button to add a new event
   - Drag the **MenuController** GameObject from the Hierarchy into the empty object field
   - Click the dropdown that says "No Function"
   - Select `MenuController` → `PlayGame()`

5. **Connect the Options Button:**

   - Select your **Options button**
   - In the Button component's "On Click ()"
   - Click `+`
   - Drag **MenuController** into the object field
   - Select `MenuController` → `OpenOptions()`

6. **Connect the Exit Button:**
   - Select your **Exit button**
   - In the Button component's "On Click ()"
   - Click `+`
   - Drag **MenuController** into the object field
   - Select `MenuController` → `ExitGame()`

---

## 🎯 Testing Your Menu

1. **Save your scene** (Ctrl+S or File → Save)
2. **Press the Play button** at the top of Unity
3. You should hear:
   - Background music playing automatically
   - A click sound when you hover and click any button
4. Test the buttons:
   - **Play**: Should load your game scene
   - **Options**: Should log a message (we'll implement this later)
   - **Exit**: Should stop Play mode in the editor

---

## 🔧 Common Issues & Solutions

### "The background music isn't playing!"

- Check that you assigned the audio clip in the Inspector
- Make sure the AudioManager GameObject is active (checkbox in Inspector)
- Check the Music Volume isn't set to 0

### "Button sounds aren't playing!"

- Make sure you assigned "menu option sfx" to EACH button
- Check that the ButtonSoundEffect component is on each button
- Verify Sfx Volume isn't 0

### "Play button doesn't load the game!"

- Check that "Game Scene Name" in MenuController matches your scene name exactly
- Make sure your game scene is added to Build Settings (File → Build Settings)

### "Exit button doesn't work!"

- In the Unity Editor, it will just stop Play mode (this is normal)
- When you build your game, it will properly close the application

---

## 🎨 Next Steps (For Later)

**For the Options Button:**

- Create a Panel in your Canvas for options (like volume sliders)
- Set it to inactive by default
- In `MenuController.OpenOptions()`, activate that panel
- You can use `MenuAudioManager.SetVolume()` to control music volume from a slider

**Making the Menu More Atmospheric:**

- Consider adding UI animations (fade-in effects)
- Add hover sound effects (different from click)
- Add creepy text animations

---

## 💡 Unity Tips for Beginners

1. **Save Often**: Ctrl+S saves your scene. Get in the habit!
2. **Test Frequently**: Click Play often to test your changes
3. **Inspector is Your Friend**: Every component has settings in the Inspector
4. **Drag & Drop**: Unity loves drag & drop. You can drag GameObjects, scripts, and assets
5. **Console Window**: If something doesn't work, check the Console (Window → General → Console) for error messages

---

Good luck with your horror game! 🎃👻
