git pull origin Working/GameFlowPrototype
git submodule update --init --remote --recursive
git submodule foreach -q --recursive "git checkout Dev;git pull origin Dev"
cmd