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
  private inputStateMappings: ActionStateTranslations;
  timeOutState?: stateId = undefined;

  constructor(
    stateId: stateId,
    actionStateTranslations: ActionStateTranslations
  ) {
    this.stateId = stateId;
    this.inputStateMappings = actionStateTranslations;
  }

  getStateForInput(stateId: stateId) {
    return this.inputStateMappings.translations.get(stateId);
  }
}

class ActionStateTranslations {
  readonly translations = new Map<gameEventId, stateId>();

  setTranslations(translations: { geId: gameEventId; sId: stateId }[]) {
    translations.forEach((actSt) => {
      this.translations.set(actSt.geId, actSt.sId);
    });
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
  const idleTranslations = new ActionStateTranslations();
  idleTranslations.setTranslations([
    { geId: GameEvents.move, sId: STATES.START_WALK },
    { geId: GameEvents.moveFast, sId: STATES.DASH },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
    { geId: GameEvents.idle, sId: STATES.IDLE },
  ]);

  return idleTranslations;
}

function InitStartWalkTranslations(): ActionStateTranslations {
  const startWalkTranslations = new ActionStateTranslations();
  startWalkTranslations.setTranslations([
    { geId: GameEvents.idle, sId: STATES.IDLE },
    { geId: GameEvents.moveFast, sId: STATES.DASH },
    { geId: GameEvents.move, sId: STATES.START_WALK },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return startWalkTranslations;
}

function InitWalkTranslations(): ActionStateTranslations {
  const walkTranslations = new ActionStateTranslations();
  walkTranslations.setTranslations([
    { geId: GameEvents.idle, sId: STATES.IDLE },
    { geId: GameEvents.move, sId: STATES.WALK },
    { geId: GameEvents.moveFast, sId: STATES.WALK },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return walkTranslations;
}

function InitDashTranslations(): ActionStateTranslations {
  const dashTranslations = new ActionStateTranslations();
  dashTranslations.setTranslations([
    { geId: GameEvents.idle, sId: STATES.IDLE },
    { geId: GameEvents.move, sId: STATES.DASH },
    { geId: GameEvents.moveFast, sId: STATES.DASH },
    { geId: GameEvents.jump, sId: STATES.JUMPSQUAT },
  ]);

  return dashTranslations;
}

function InitJumpSquatTranslations(): ActionStateTranslations {
  const jumpSquatTranslations = new ActionStateTranslations();
  jumpSquatTranslations.setTranslations([]);

  return jumpSquatTranslations;
}
