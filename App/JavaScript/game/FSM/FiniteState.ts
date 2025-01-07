import { GameEvents } from '../events/events';

// TYPES AND CLASSES ====================================

type gameEventId = number;
type stateId = number;

const STATES = {
  IDLE: 0 as stateId,
  START_WALK: 1 as stateId,
  WALK: 2 as stateId,
  DASH: 3 as stateId,
  RUN: 4 as stateId,
  JUMPSQUAT: 5 as stateId,
  JUMP: 6 as stateId,
  NFALL: 7 as stateId,
  FFALL: 8 as stateId,
  LAND: 9 as stateId,
  SOFTLAND: 10 as stateId,
};

class StateRelation {
  readonly stateId: stateId = STATES.IDLE;
  private inputStateMappings: ActionStateMappings;
  timeOutState?: stateId = undefined;

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

  setMappings(mappingsArray: { geId: gameEventId; sId: stateId }[]) {
    mappingsArray.forEach((actSt) => {
      this.mappings.set(actSt.geId, actSt.sId);
    });
  }

  getMapping(geId: gameEventId): stateId | undefined {
    return this.mappings.get(geId);
  }
}

class FSMState {
  readonly stateFrameLength?: number = undefined;
  readonly stateId: stateId;
  onEnter?: () => void;
  onUpdate?: () => void;
  onExit?: () => void;

  constructor(stateId: stateId, stateFrameLength: number | undefined) {
    this.stateFrameLength = stateFrameLength;
    this.stateId = stateId;
  }
}

// STATE RELATIONS ===================================================

const IDLE_STATE_RELATIONS = InitIdleRelations();
const START_WALK_STATE_RELATIONS = InitStartWalkRelations();

function InitIdleRelations(): StateRelation {
  const idle = new StateRelation(STATES.IDLE, InitIdleTranslations());
  return idle;
}

function InitStartWalkRelations(): StateRelation {
  const startWalk = new StateRelation(
    STATES.START_WALK,
    InitStartWalkTranslations()
  );

  startWalk.timeOutState = STATES.WALK;

  return startWalk;
}

function InitWalkRelations(): StateRelation {
  const walkRelations = new StateRelation(STATES.WALK, InitWalkTranslations());

  return walkRelations;
}

function InitDashRelations(): StateRelation {
  const dashRelations = new StateRelation(STATES.DASH, InitDashTranslations());
  dashRelations.timeOutState = STATES.RUN;

  return dashRelations;
}

function InitIdleTranslations() {
  const idleTranslations = new ActionStateMappings();
  idleTranslations.setMappings([
    { geId: GameEvents.move, sId: STATES.START_WALK },
    { geId: GameEvents.moveFast, sId: STATES.DASH },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return idleTranslations;
}

function InitStartWalkTranslations(): ActionStateMappings {
  const startWalkTranslations = new ActionStateMappings();
  startWalkTranslations.setMappings([
    { geId: GameEvents.idle, sId: STATES.IDLE },
    { geId: GameEvents.moveFast, sId: STATES.DASH },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return startWalkTranslations;
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

  return dashTranslations;
}

function InitJumpSquatTranslations(): ActionStateMappings {
  const jumpSquatTranslations = new ActionStateMappings();

  return jumpSquatTranslations;
}

function InitJumpRelations(): ActionStateMappings {
  const jumpTranslations = new ActionStateMappings();
  jumpTranslations.setMappings([{ geId: GameEvents.jump, sId: STATES.JUMP }]);

  return jumpTranslations;
}
