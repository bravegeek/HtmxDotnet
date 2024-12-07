const canvas = document.getElementById('fsmCanvas') as HTMLCanvasElement;
const ctx = canvas.getContext('2d')!;
const radius = 30;

// State management =====================================================
const states: State[] = [];
let triggers: string[] = ['idle', 'test'];
let isConnecting = false;
let selectedState: State | null = null;
let isDragging = false;
let isEditing = false;

interface Connection {
  target: State;
  trigger: string;
}

interface Attribute {
  name: string;
  type: string;
  value: string;
}

interface StateData {
  id: number;
  name: string;
  frameCount: number;
  attributes: Attribute[];
}

class State {
  data: StateData;
  x: number;
  y: number;
  connections: Connection[];
  isBeingDragged: boolean = false;
  isHovered: boolean = false;
  color = 'white';

  constructor(id: number, name: string, x: number, y: number) {
    this.data = {
      id,
      name,
      frameCount: 20,
      attributes: [],
    };
    this.x = x;
    this.y = y;
    this.connections = [];
  }

  draw(): void {
    // Draw state (circle)
    ctx.beginPath();
    ctx.arc(this.x, this.y, radius, 0, Math.PI * 2);
    ctx.fillStyle = this.color;
    ctx.fill();
    ctx.strokeStyle = 'black';
    ctx.stroke();
    ctx.closePath();

    // Draw name
    ctx.fillStyle = 'black';
    ctx.font = '14px Arial';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    ctx.fillText(this.data.name, this.x, this.y);
  }

  connectTo(target: State, trigger: string): void {
    this.connections.push({ target, trigger });
  }

  drawConnections(): void {
    this.connections.forEach(({ target, trigger }) => {
      // Draw arrow
      const dx = target.x - this.x;
      const dy = target.y - this.y;
      const angle = Math.atan2(dy, dx);

      const startX = this.x + radius * Math.cos(angle);
      const startY = this.y + radius * Math.sin(angle);
      const endX = target.x - radius * Math.cos(angle);
      const endY = target.y - radius * Math.sin(angle);

      ctx.beginPath();
      ctx.moveTo(startX, startY);
      ctx.lineTo(endX, endY);
      ctx.strokeStyle = 'blue';
      ctx.stroke();
      ctx.closePath();

      // Draw trigger label
      const midX = (startX + endX) / 2;
      const midY = (startY + endY) / 2;
      ctx.fillStyle = 'red';
      ctx.fillText(trigger, midX, midY);
    });
  }
}

// Modes =====================================================

function connectionMode(clickedState: State) {
  if (!selectedState && clickedState) {
    selectedState = clickedState;
    alert(
      `Selected ${clickedState.data.name}. Now click another state to connect.`
    );
  } else if (selectedState && clickedState && selectedState !== clickedState) {
    const trigger = prompt('Enter trigger for this connection:');
    if (trigger) {
      selectedState.connectTo(clickedState, trigger);
    }
    selectedState = null;
  }
}

function editMode(clickedState: State) {
  console.log(clickedState);
}

// Untils ===============================================================

function setDraggingFalse() {
  states.forEach((x) => (x.isBeingDragged = false));
  isDragging = false;
}

function mouseOverState(x: number, y: number): State | undefined {
  return states.find((state) => Math.hypot(state.x - x, state.y - y) < radius);
}

function dragging(x: number, y: number) {
  const stateBeingDragged = states.find((x) => x.isBeingDragged);

  stateBeingDragged!.x = x;
  stateBeingDragged!.y = y;
}

function getElementById(id: string): HTMLElement | null {
  return document.getElementById(id);
}

// Render functions =====================================================

function render(): void {
  drawCanvas();
  requestAnimationFrame(render);
}

function drawCanvas(): void {
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  // Draw connections first
  states.forEach((state) => state.drawConnections());

  // Draw states
  states.forEach((state) => state.draw());
}

// Event Listeners =====================================================

getElementById('addState')!.addEventListener('click', () => {
  const id = states.length + 1;
  const name = `State ${id}`;
  const x = Math.random() * (canvas.width - 60) + radius;
  const y = Math.random() * (canvas.height - 60) + radius;
  const state = new State(id, name, x, y);
  states.push(state);
});

// Edit Mode listener
getElementById('editState')!.addEventListener('click', () => {
  isEditing = !isEditing;
  isConnecting = false;
});

// Connect states handler
getElementById('connectStates')!.addEventListener('click', () => {
  isConnecting = !isConnecting;
  selectedState = null;
  isEditing = false;
  alert(
    isConnecting ? 'Click two states to connect them.' : 'Connection mode off.'
  );
});

// Clear canvas handler
getElementById('clearCanvas')!.addEventListener('click', () => {
  states.length = 0;
});

getElementById('addTrigger')!.addEventListener('click', (e) => {
  const inp = getElementById('trigger-input') as HTMLInputElement;
  const trig = inp.value;

  if (triggers.includes(trig)) {
    return;
  }

  triggers.push(trig);
  triggers.sort();
  console.log(triggers);
  renderTriggers();
});

canvas.addEventListener('click', (event: MouseEvent) => {
  const { offsetX, offsetY } = event;

  // Check if clicking a state
  const clickedState = states.find(
    (state) => Math.hypot(state.x - offsetX, state.y - offsetY) < radius
  );

  if (isConnecting && clickedState) {
    connectionMode(clickedState);
    return;
  }

  if (isEditing && clickedState) {
    editMode(clickedState);
  }
});

canvas.addEventListener('mousedown', (event: MouseEvent) => {
  const { offsetX, offsetY } = event;
  const clickedAndHoldState = mouseOverState(offsetX, offsetY);

  if (clickedAndHoldState && !isEditing && !isConnecting) {
    clickedAndHoldState.x = offsetX;
    clickedAndHoldState.y = offsetY;
    clickedAndHoldState.isBeingDragged = true;
    isDragging = true;
  } else {
    setDraggingFalse();
  }
});

canvas.addEventListener('mousemove', (event: MouseEvent) => {
  if (!isDragging) {
    return;
  }
  const { offsetX, offsetY } = event;
  dragging(offsetX, offsetY);
});

canvas.addEventListener('mouseup', (event: MouseEvent) => {
  setDraggingFalse();
});

canvas.addEventListener('mousemove', (event: MouseEvent) => {
  if (isEditing || isConnecting) {
    const { offsetX, offsetY } = event;
    let anyHovered = false;

    const overState = mouseOverState(offsetX, offsetY);
    if (overState) {
      overState.isHovered = true;
      overState.color = 'green';
      anyHovered = true;
    } else {
      states.forEach((s) => {
        s.color = 'white';
        s.isHovered = false;
      });
    }

    if (anyHovered) {
      canvas.style.cursor = 'pointer';
    } else {
      canvas.style.cursor = 'default';
    }
  }
});

// UI ======================================================

function removeTrigger(triggerId: string) {
  triggers = triggers.filter((t) => t != triggerId);
  renderTriggers();
}

function renderTriggers() {
  const triggerContainer = getElementById('existing-triggers')!;
  triggerContainer.innerHTML = '';

  const ul = document.createElement('ul');

  triggers.forEach((t) => {
    const li = document.createElement('li');
    li.innerText = t;

    const removeBtn = document.createElement('button');

    removeBtn.classList.add('btn');
    removeBtn.classList.add('btn-danger');
    removeBtn.id = `remove-${t}`;

    removeBtn.onclick = (e: Event) => {
      const trgt = e.target as HTMLButtonElement;
      console.log(trgt.id);
      const id = trgt.id;
      const triggerName = id.slice(id.lastIndexOf('-') + 1, id.length);
      removeTrigger(triggerName);
    };

    removeBtn.innerText = 'X';

    li.appendChild(removeBtn);
    ul.appendChild(li);
  });

  triggerContainer.appendChild(ul);
}

// Top Level Calls =========================================
render();

renderTriggers();
