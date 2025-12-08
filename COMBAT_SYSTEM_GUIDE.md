# 🎮 Guía: Sistema de Vida + NPC Disparador

## 📊 **PARTE 1: Crear la Barra de Vida (UI)**

### **Paso 1: Crear la estructura de UI**

1. **Haz clic derecho en la Jerarquía** → UI → Canvas (si no tienes uno ya)
2. **Haz clic derecho en el Canvas** → UI → Image
   - Renómbrala a "HealthBar_Background"
   - Esta será el fondo de la barra
3. **Haz clic derecho en HealthBar_Background** → UI → Image
   - Renómbrala a "HealthBar_Fill"
   - Esta será la parte que se llena/vacía

### **Paso 2: Configurar el fondo (Background)**

1. Selecciona **HealthBar_Background**
2. En **Rect Transform**:
   - Width: 200
   - Height: 20
   - Posición: Esquina superior izquierda (ajusta a tu gusto)
3. En **Image**:
   - Color: Negro o gris oscuro
   - Puedes usar tu sprite de vida aquí si quieres

### **Paso 3: Configurar el Fill (la barra que se mueve)**

1. Selecciona **HealthBar_Fill**
2. En **Rect Transform**:
   - **Anchors**: Establecer en Left-Stretch (Alt+Shift+Click en el botón de anchors, luego selecciona left-middle)
   - Left: 0, Right: 0, Top: 0, Bottom: 0 (para que llene todo el padre)
3. En **Image**:
   - Color: Verde brillante (#00FF00 o similar)
   - **Image Type**: Filled
   - **Fill Method**: Horizontal
   - **Fill Origin**: Left
   - **Fill Amount**: 1 (completamente llena)

### **Paso 4: Agregar el script HealthBar**

1. Selecciona **HealthBar_Background** (el padre)
2. **Add Component** → Busca "HealthBar"
3. En el componente HealthBar:
   - **Fill Image**: Arrastra **HealthBar_Fill** aquí
   - **Use Color Gradient**: ✓ (marcado)
   - **Smooth Speed**: 5

### **Paso 5: Configurar el Gradient (opcional pero recomendado)**

1. En el componente HealthBar, verás **Color Gradient**
2. Haz clic en él para abrir el editor de gradientes
3. Configura los colores:
   - **0%** (izquierda): Rojo (#FF0000) - vida baja
   - **50%** (medio): Amarillo (#FFFF00) - vida media
   - **100%** (derecha): Verde (#00FF00) - vida alta

### **Paso 6: Conectar al Player**

1. Selecciona tu **Player** en la Jerarquía
2. En el script **Player**:
   - **Health Bar**: Arrastra el GameObject **HealthBar_Background** aquí

---

## 🎯 **PARTE 2: Crear el Sistema de Proyectiles**

### **Paso 1: Crear el Prefab del Proyectil**

1. **Crear el GameObject**:
   - Jerarquía → Create Empty → Renombrar a "Projectile"
2. **Agregar visual**:
   - Add Component → Sprite Renderer
   - Asigna un sprite (puede ser un círculo, bala, etc.)
   - Color: Rojo o lo que quieras
   - Si no tienes sprite, crea uno:
     - Jerarquía → 2D Object → Sprite → Circle
     - Hazlo hijo de Projectile
     - Escala pequeña (0.2, 0.2, 1)
3. **Agregar física**:
   - Add Component → Circle Collider 2D (o Box Collider 2D)
   - **Is Trigger**: ✓ (IMPORTANTE - debe estar marcado)
   - Ajusta el tamaño del collider
4. **Agregar el script**:
   - Add Component → Busca "Projectile"
   - **Speed**: 10
   - **Damage**: 10
   - **Lifetime**: 5
5. **Crear el Prefab**:
   - Crea una carpeta "Prefabs" en Assets
   - Arrastra el GameObject "Projectile" desde la Jerarquía a la carpeta Prefabs
   - Ahora puedes eliminar el Projectile de la escena

### **Paso 2: Verificar el Player**

1. Selecciona tu **Player**
2. Asegúrate de que tenga:
   - **Tag**: "Player" (en la parte superior del Inspector)
   - **Collider 2D** (el que ya tienes)
   - **Is Trigger**: Depende de tu juego, pero normalmente NO

---

## 👾 **PARTE 3: Configurar el NPC Disparador**

### **Paso 1: Crear Fire Point (punto de disparo)**

1. Selecciona tu **NPC** en la Jerarquía
2. **Clic derecho en NPC** → Create Empty
3. Renombrarlo a "FirePoint"
4. Moverlo a donde quieras que salgan los proyectiles (generalmente frente al NPC)

### **Paso 2: Configurar el NPC**

1. Selecciona el **NPC**
2. Verifica que tenga el script **NPC** adjunto
3. En el componente NPC:
   - **Player**: Arrastra tu **Player** desde la Jerarquía (o déjalo vacío, lo buscará automáticamente)
   - **Projectile Prefab**: Arrastra el **Projectile** desde la carpeta Prefabs
   - **Fire Point**: Arrastra el **FirePoint** (hijo del NPC)
   - **Fire Rate**: 2 (dispara cada 2 segundos)
   - **Detection Range**: 15 (dispara si el jugador está a 15 unidades o menos)
   - **Predict Player Movement**: ✓ (opcional - hace la IA más inteligente)
   - **Projectile Speed**: 10 (debe coincidir con la velocidad del proyectil)

### **Paso 3: Verificar Tags**

1. Selecciona tu **Player**
2. En Inspector, arriba, donde dice **Tag**:
   - Selecciona "Player" (si no existe, créalo: Add Tag... → + → "Player")

---

## 🧪 **PARTE 4: Probar el Sistema**

### **Prueba en el Editor:**

1. **Dale Play** en Unity
2. **Acércate al NPC** (dentro de la distancia de detección)
3. El NPC debería:
   - ✅ Disparar proyectiles cada 2 segundos
   - ✅ Apuntar hacia ti
4. Cuando un proyectil te toque:
   - ✅ La barra de vida debería bajar
   - ✅ El color debería cambiar (verde → amarillo → rojo)
   - ✅ Verás un mensaje en la Console

### **Visualización en el Editor (sin dar Play):**

1. Selecciona el **NPC**
2. Verás un círculo rojo alrededor (el rango de detección)
3. Si el Player está asignado, verás una línea amarilla hacia él

---

## 🎨 **MEJORAS OPCIONALES**

### **Mejorar la Barra de Vida:**

1. **Agregar borde**:
   - Clic derecho en HealthBar_Background → UI → Image
   - Renombrar a "Border"
   - Configurar como Outline o usar un sprite de marco

2. **Agregar texto de vida**:
   - Clic derecho en HealthBar_Background → UI → Text (TextMeshPro)
   - Mostrar "100/100"

### **Mejorar los Proyectiles:**

1. **Agregar efecto visual**:
   - Add Component → Particle System
   - Crear un trail o efecto de fuego

2. **Agregar sonido**:
   - Add Component → Audio Source
   - Asignar un sonido de disparo
   - Play On Awake: ✓

### **Mejorar el NPC:**

1. **Animación de disparo**:
   - En el método `Shoot()` puedes agregar:
   ```csharp
   animator.SetTrigger("Shoot");
   ```

2. **Efecto de disparo**:
   - Instantiate un efecto de partículas en el FirePoint

---

## ⚠️ **Problemas Comunes**

### "Los proyectiles no dañan al jugador"
- Verifica que el proyectil tenga **Is Trigger** marcado
- Verifica que el Player tenga el tag "Player"
- Verifica que el proyectil tenga el script **Projectile**

### "La barra de vida no se actualiza"
- Verifica que hayas asignado HealthBar_Fill en el script HealthBar
- Verifica que hayas asignado HealthBar en el script Player

### "El NPC no dispara"
- Verifica que el Projectile Prefab esté asignado
- Verifica que el Player esté dentro del Detection Range
- Revisa la Console por errores

### "Los proyectiles atraviesan paredes"
- Si quieres que se destruyan al tocar paredes:
  - Asigna el tag "Wall" o "Ground" a tus paredes
  - El script Projectile ya maneja esto

---

## 📝 **Resumen de Scripts Creados**

1. **HealthBar.cs** → Va en el GameObject de la barra de vida UI
2. **Projectile.cs** → Va en el prefab del proyectil
3. **NPC.cs** → Va en el enemigo que dispara
4. **Player.cs** → Ya lo tenías, le agregué la referencia a HealthBar

---

¡Con esto ya tienes un sistema completo de combate! El jugador debe esquivar los proyectiles o perderá vida. 🎮👾
