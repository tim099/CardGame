git pull origin
git submodule update --init --remote --recursive
git submodule foreach -q --recursive "git checkout Dev;git pull origin Dev"
cmd