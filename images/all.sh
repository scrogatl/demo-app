#! /bin/zsh

REPOSITORY_PREFIX=dambott/x64
if [ -z "${REPOSITORY_PREFIX}" ]
then 
    echo "Please set the REPOSITORY_PREFIX"
else 
	for f in *; do
		if [ -d "$f" ]; then
			echo 
			echo "Building $f..."
			echo 
			./build.sh $f
			echo 
			echo "Finished building $f!"
			echo 
		fi 
	done
fi
