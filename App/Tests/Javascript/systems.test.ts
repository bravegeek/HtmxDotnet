import { ZERO_CR_POOL } from '../../JavaScript/game/engine/physics/collisions';
import { ZERO_POOL } from '../../JavaScript/game/engine/physics/vector';
import { Player } from '../../JavaScript/game/engine/player/playerOrchestrator';
import {
  defaultStage,
  StageOrchestrator,
} from '../../JavaScript/game/engine/stage/stageComponents';
import { StageCollisionDetection } from '../../JavaScript/game/engine/systems/systems';

beforeEach(() => {
  ZERO_POOL();
  ZERO_CR_POOL();
});

test('stage collision ground', () => {
  const stage = defaultStage();
  const p = new Player();
  p.SetPlayerPostion(700, 455.0);
  const collided = StageCollisionDetection(p, stage);

  expect(collided).toBeTruthy();
});

test('stage collision right wall', () => {
  const stage = defaultStage();
  const p = new Player();
  p.SetPlayerPostion(555, 525);

  const collided = StageCollisionDetection(p, stage);
  expect(collided).toBeTruthy();
});

test('stage collision corner case', () => {
  const stage = defaultStage();
  const p = new Player();
  p.SetPlayerPostion(595, 460);
  const colided = StageCollisionDetection(p, stage);

  expect(colided).toBeTruthy();
});
