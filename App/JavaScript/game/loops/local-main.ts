import { Jazz } from '../engine/jazz';
import { NewMessageFromLocalInput } from '../network/protocol';
import { DebugRenderer, RenderData, resolution } from '../render/debug-2d';
import { RENDERFPS60Loop } from './FPS60LoopExecutor';
import { GetInput, listenForGamePadInput } from './Input';

let localFrame = 0;
let renderData = new RenderData();
const frameInterval = 1000 / 60;

export function start() {
  debugger;
  INPUT_LOOP();
  LOGIC_LOOP();
  RENDER_LOOP();
}

function INPUT_LOOP() {
  const p1Controller = getPlayerControllerIndex();
  listenForGamePadInput(p1Controller);
}

function LOGIC_LOOP() {
  const engine = new Jazz((newRdDto: RenderData) => {
    renderData.frame = newRdDto.frame;
    renderData.frameTime = newRdDto.frameTime;
  });

  const logicLoopHandle = setInterval(() => {
    logicStep(engine);
  }, frameInterval);
}

function RENDER_LOOP() {
  const canvas = document.getElementById('game') as HTMLCanvasElement;
  const resolution: resolution = { x: 1920, y: 1080 };
  const dbRenderer = new DebugRenderer(canvas, resolution);
  RENDERFPS60Loop(() => {
    dbRenderer.render(renderData);
  });
}

function getPlayerControllerIndex(): number {
  return 0;
}

function logicStep(engine: Jazz) {
  const input = GetInput();
  const message = NewMessageFromLocalInput(input, localFrame);
  engine.postMessage(message);

  localFrame++;
}
