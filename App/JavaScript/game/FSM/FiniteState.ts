import { GameEvents } from '../events/events';

// TYPES AND CLASSES ====================================

type gameEventId = number;
type stateId = number;

const STATES = {
  IDLE: 0 as stateId,
  START_WALK: 1 as stateId,
  TURN_WALK: 2 as stateId,
  WALK: 3 as stateId,
  DASH: 4 as stateId,
  STOP_RUN: 5 as stateId,
  STOP_RUN_TURN: 6 as stateId,
  RUN: 7 as stateId,
  JUMPSQUAT: 8 as stateId,
  JUMP: 9 as stateId,
  NFALL: 10 as stateId,
  FFALL: 11 as stateId,
  LAND: 12 as stateId,
  SOFTLAND: 13 as stateId,
};

class StateRelation {
  readonly stateId: stateId = STATES.IDLE;
  private inputStateMappings: ActionStateMappings;

  constructor(stateId: stateId, actionStateTranslations: ActionStateMappings) {
    this.stateId = stateId;
    this.inputStateMappings = actionStateTranslations;
  }

  getStateForInput(geId: gameEventId) {
    return this.inputStateMappings.getMapping(geId);
  }
}

class ActionStateMappings {
  private readonly mappings = new Map<gameEventId, stateId>();
  private defaultSate?: stateId;

  setMappings(mappingsArray: { geId: gameEventId; sId: stateId }[]) {
    mappingsArray.forEach((actSt) => {
      this.mappings.set(actSt.geId, actSt.sId);
    });
  }

  getMapping(geId: gameEventId): stateId | undefined {
    return this.mappings.get(geId);
  }

  getDefault(): stateId | undefined {
    return this.defaultSate;
  }

  _setDefault(stateId: stateId) {
    if (!this.defaultSate) {
      this.defaultSate = stateId;
    }
  }
}

type stateConfig = {
  onEnter?: () => void;
  onUpdate?: () => void;
  onExit?: () => void;
};

export class FSMState {
  readonly stateId: stateId;
  readonly stateName: string;
  readonly stateFrameLength?: number = undefined;
  readonly interuptFrame?: number = undefined;
  onEnter?: () => void;
  onUpdate?: () => void;
  onExit?: () => void;

  constructor(
    stateId: stateId,
    stateName: string,
    config: stateConfig,
    stateFrameLength: number | undefined = undefined,
    interuptFrame: number | undefined = undefined
  ) {
    this.stateFrameLength = stateFrameLength;
    this.interuptFrame = interuptFrame;
    this.stateName = stateName;
    this.stateId = stateId;
    this.onEnter = config.onEnter;
    this.onExit = config.onExit;
    this.onUpdate = config.onUpdate;
  }

  canInterupt(currentFrame: number): boolean {
    if (this.interuptFrame == undefined) {
      return true;
    }

    if (currentFrame >= this.interuptFrame) {
      return true;
    }

    return false;
  }
}

// STATE RELATIONS ===================================================

const IDLE_STATE_RELATIONS = InitIdleRelations();
const START_WALK_RELATIONS = InitStartWalkRelations();
const TURN_WALK_RELATIONS = InitTurnWalkRelations();
const DASH_RELATIONS = InitDashRelations();

function InitIdleRelations(): StateRelation {
  const idle = new StateRelation(STATES.IDLE, InitIdleTranslations());
  return idle;
}

function InitStartWalkRelations(): StateRelation {
  const startWalk = new StateRelation(
    STATES.START_WALK,
    InitStartWalkTranslations()
  );

  return startWalk;
}

function InitTurnWalkRelations(): StateRelation {
  const turnWalk = new StateRelation(
    STATES.TURN_WALK,
    InitTurnWalkTranslations()
  );

  return turnWalk;
}

function InitWalkRelations(): StateRelation {
  const walkRelations = new StateRelation(STATES.WALK, InitWalkTranslations());

  return walkRelations;
}

function InitDashRelations(): StateRelation {
  const dashRelations = new StateRelation(STATES.DASH, InitDashTranslations());

  return dashRelations;
}

function InitIdleTranslations() {
  const idleTranslations = new ActionStateMappings();
  idleTranslations.setMappings([
    { geId: GameEvents.move, sId: STATES.START_WALK },
    { geId: GameEvents.moveFast, sId: STATES.DASH },
    { geId: GameEvents.turn, sId: STATES.TURN_WALK },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return idleTranslations;
}

function InitStartWalkTranslations(): ActionStateMappings {
  const startWalkTranslations = new ActionStateMappings();
  startWalkTranslations.setMappings([
    { geId: GameEvents.moveFast, sId: STATES.DASH },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  startWalkTranslations._setDefault(STATES.IDLE);

  return startWalkTranslations;
}

function InitTurnWalkTranslations(): ActionStateMappings {
  const turnWalkTranslations = new ActionStateMappings();
  turnWalkTranslations.setMappings([
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return turnWalkTranslations;
}

function InitWalkTranslations(): ActionStateMappings {
  const walkTranslations = new ActionStateMappings();
  walkTranslations.setMappings([
    { geId: GameEvents.idle, sId: STATES.IDLE },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return walkTranslations;
}

function InitDashTranslations(): ActionStateMappings {
  const dashTranslations = new ActionStateMappings();
  dashTranslations.setMappings([
    { geId: GameEvents.idle, sId: STATES.IDLE },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  dashTranslations._setDefault(STATES.RUN);

  return dashTranslations;
}

function InitRunTranslations(): ActionStateMappings {
  const runTranslations = new ActionStateMappings();
  runTranslations.setMappings([
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
    { geId: GameEvents.idle, sId: STATES.STOP_RUN },
    { geId: GameEvents.turn, sId: STATES.STOP_RUN_TURN },
  ]);

  return runTranslations;
}

function InitJumpSquatTranslations(): ActionStateMappings {
  const jumpSquatTranslations = new ActionStateMappings();
  jumpSquatTranslations._setDefault(STATES.JUMP);

  return jumpSquatTranslations;
}

function InitJumpTranslations(): ActionStateMappings {
  const jumpTranslations = new ActionStateMappings();
  jumpTranslations.setMappings([{ geId: GameEvents.jump, sId: STATES.JUMP }]);

  return jumpTranslations;
}
